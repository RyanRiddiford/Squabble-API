using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleHashing;
using Squabble.Data;
using Squabble.Helpers;
using Squabble.Managers;
using Squabble.Models;
using Squabble.Models.Entities;
using Squabble.Models.RequestModels;

namespace Squabble.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly AccountManager _accountManager;
        private readonly SquabbleContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AccountManager accountManager, SquabbleContext context, ILogger<AccountController> logger)
        {
            _accountManager = accountManager;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get the user account.
        /// </summary>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public User Get()
        {
            return _accountManager.GetById(SupportHelpers.FindIdFromToken(HttpContext.User.Claims));
        }

        [HttpGet]
        [Route("[action]/{username}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public User GetByUserName(string username)
        {
            return _accountManager.GetByUsername(username);
        }

        [HttpGet]
        [Route("[action]/{email}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public User GetByEmail(string email)
        {
            return _accountManager.GetByEmail(email);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public User GetByCommunicationUserId(string id)
        {
            return _accountManager.GetByCommunicationUserId(id);
        }

        /// <summary>
        /// Update account info.
        /// </summary>
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Put([FromBody] UpdateAccountRequest updateAccountRequest)
        {
            var login = await _context.Logins
                .Include(l => l.Account)
                .Where(l => l.Account.UserName == updateAccountRequest.Username)
                .FirstOrDefaultAsync();

            if (login == null)
            {
                _logger.LogWarning($"{updateAccountRequest.Username} attempted to update an account that does not exist.");
                return BadRequest("Account does not exist.");
            }

            if (login.Account.IsSso && updateAccountRequest.Password != null)
            {
                return BadRequest("Accounts using SSO don't have passwords.");
            }

            if (updateAccountRequest.Password != updateAccountRequest.ConfirmPassword)
            {
                _logger.LogWarning($"UN: {updateAccountRequest.Username} | EM: {updateAccountRequest.Email} failed to confirm password.");
                return BadRequest("Password Mismatch");
            }

            if (updateAccountRequest.Password != null)
            {
                login.PasswordHash = PBKDF2.Hash(updateAccountRequest.Password);
            }

            if (updateAccountRequest.FirstName != null)
            {
                login.Account.FirstName = updateAccountRequest.FirstName;
            }

            if (updateAccountRequest.MiddleName != null)
            {
                login.Account.MiddleName = updateAccountRequest.MiddleName;
            }

            if (updateAccountRequest.Surname != null)
            {
                login.Account.Surname = updateAccountRequest.Surname;
            }

            if (updateAccountRequest.AvatarString != null)
            {
                login.Account.Avatar = updateAccountRequest.AvatarString;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async void Post([FromBody] User user)
        {
            await _accountManager.Add(user);
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public long Delete()
        {
            return _accountManager.Delete(SupportHelpers.FindIdFromToken(HttpContext.User.Claims));
        }
    }
}
