using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SimpleHashing;
using Squabble.Data;
using Squabble.Helpers;
using Squabble.Managers;
using Squabble.Models;
using Squabble.Models.Entities;
using Squabble.Models.RequestModels;
using Squabble.Models.ResponseModels;

namespace Squabble.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AccountManager _accountManager;
        private readonly LoginManager _loginManager;
        private readonly SquabbleContext _context;
        private readonly CommunicationTokenManager _commTokenManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(
            AccountManager accountManager,
            LoginManager loginManager,
            SquabbleContext context,
            CommunicationTokenManager tokenManager,
            ILogger<LoginController> logger
        )
        {
            _loginManager = loginManager;
            _accountManager = accountManager;
            _context = context;
            _commTokenManager = tokenManager;
            _logger = logger;
        }

        //Attempt to create a new user account through local provider
        [HttpPost, Route("[action]")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest model)
        {
            if (model.MicrosoftSsoId == null && model.Password != model.ConfirmPassword)
            {
                return BadRequest("Password Mismatch");
            }

            if (!ModelState.IsValid) return BadRequest();

            var acsData = await _commTokenManager.CreateUserAndTokenAsync();

            var account = new User
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                Surname = model.Surname,
                Email = model.Email,
                CommunicationUserId = acsData.User.Id,
                CommunicationToken = acsData.AccessToken.Token
            };

            if (model.MicrosoftSsoId != null)
            {
                account.IsSso = true;
                account.MicrosoftSsoId = model.MicrosoftSsoId;
            }

            //Attempt to add new account to database
            int? accountResult = null;
            try
            {
                accountResult = await _accountManager.Add(account);
            }
            catch (Exception e)
            {
                _logger.LogError($"{accountResult} failed to be added to the database.");
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return null;
            }
           
            

            if (!accountResult.HasValue) return BadRequest();

            var login = new Login
            {
                AccountId = (int)accountResult,
                Email = model.Email,
                UserName = model.UserName
            };

            if (model.MicrosoftSsoId == null)
            {
                login.PasswordHash = PBKDF2.Hash(model.Password);
                login.SecurityQuestionOne = model.SecurityQuestionOne;
                login.SecurityAnswerOne = PBKDF2.Hash(model.SecurityAnswerOne);
                login.SecurityQuestionTwo = model.SecurityQuestionTwo;
                login.SecurityAnswerTwo = PBKDF2.Hash(model.SecurityAnswerTwo);
            }

            // Attempt to add new login to database.
            try
            {
                await _loginManager.Add(login);
            } catch (Exception e)
            {
                _logger.LogError($"{login.AccountId} failed to be added to the database.");
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return null;
            }
            

            return InitToken(account.AccountId, model.UserName);
        }

        //Returns a JWT with an expiry date
        [NonAction]
        private IActionResult InitToken(int accountId, string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtHelpers.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("account_id", accountId.ToString()),
                new Claim("username", username)
            };

            var token = new JwtSecurityToken(
                JwtHelpers.Issuer,
                JwtHelpers.Audience,
                claims,
                expires: DateTime.UtcNow.AddHours(JwtHelpers.ExpiresIn),
                signingCredentials: credentials
            );
            string acsId = "";
            string acsToken = "";
            //Get acs id to return on login/register for acs initialisation steps
            try
            {
                acsId = _context.Accounts.Find(accountId).CommunicationUserId;
                acsToken = _context.Accounts.Find(accountId).CommunicationToken;
            } catch(Exception e)
            {
                _logger.LogError($"Accounts with {acsId} or {acsToken} could not be found.");
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return null;
            }
          

            var results = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                account_id = accountId,
                acs_id = acsId,
                acs_token = acsToken
            };

            return Created("", results);
        }


        // Verifies the login credentials and logs the user in
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (model.Sso)
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(x => x.MicrosoftSsoId == model.MicrosoftSsoId);

                if (account != null) return InitToken(account.AccountId, account.UserName);

                var response = new
                {
                    redirectTo = "/register",
                    queryParams = new
                    {
                        sso = true,
                        microsoftSsoId = model.MicrosoftSsoId
                    }
                };

                return Ok(response);
            }

            // Attempt to authenticate non-SSO sign-in
            if (model.UserName == "" || model.Password == "")
            {
                return BadRequest("Username and password are both required.");
            }

            var accountId = await _loginManager.VerifyCredentials(model);

            if (accountId.HasValue)
            {
                return Ok(InitToken((int)accountId, model.UserName));
            }

            return BadRequest("Invalid login credentials");
        }

        /// <summary>
        /// Endpoint to reset a user password.
        /// </summary>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid request.");

            // If password and confirmation password don't match return client error.
            if (!model.Password.Equals(model.ConfirmPassword)) return BadRequest("Passwords do not match.");

            // Fetch the login from the DB.
            var login = await _context.Logins.FirstOrDefaultAsync(x => x.UserName == model.UserName);

            // If login doesn't exist, throw an error.
            if (login == null) return BadRequest("Username doesn't exist.");

            if (
                !PBKDF2.Verify(login.SecurityAnswerOne, model.SecurityAnswerOne) ||
                !PBKDF2.Verify(login.SecurityAnswerTwo, model.SecurityAnswerTwo)
            )
            {
                _logger.LogError($"UN: {login.UserName} | EM: {login.Email} has failed to answer the security questions.");
                return BadRequest("Security answer does not match.");
            }

            // Reset the user password.
            login.PasswordHash = PBKDF2.Hash(model.Password);

            // Persist changes.
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"{login.AccountId} failed to be updated to the database.");
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return BadRequest("Error adding to the database.");
            }

            return Ok();
        }

        /// <summary>
        /// Endpoint to get security questions for a user.
        /// </summary>
        [HttpGet]
        [Route("[action]/{username}")]
        public async Task<IActionResult> GetSecurityQuestions(string username)
        {
            // Fetch the login from the DB.
            var login = await _context.Logins.FirstOrDefaultAsync(x => x.UserName == username);

            // If login doesn't exist, throw an error.
            if (login == null) return BadRequest("Username doesn't exist.");

            var securityQuestions = new SecurityQuestionsResponse
            {
                SecurityQuestionOne = login.SecurityQuestionOne,
                SecurityQuestionTwo = login.SecurityQuestionTwo
            };

            return Ok(securityQuestions);
        }
    }
}
