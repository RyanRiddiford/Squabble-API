using Azure;
using Azure.Communication;
using Azure.Communication.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Squabble.Managers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squabble.Controllers;
using Squabble.Helpers;

namespace Squabble.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTokenController : Controller
    {
        public CommunicationTokenManager _acsManager { get; set; }
        private AccountManager _accountManager { get; set; }

        public UserTokenController(CommunicationTokenManager acsManager, AccountManager accountManager)
        {
            _acsManager = acsManager;
            _accountManager = accountManager;
        }

        /// <summary>
        /// Create a User.
        /// </summary>
        /// <returns>The user id.</returns>
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetUserAsync()
        {
            var user = await _acsManager.CreateUserAsync();
            try
            {
                var jsonFormattedUser = new
                {
                    communicationUserId = user.Id
                };

                return this.Ok(jsonFormattedUser);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error occured while creation user: {ex}");
                return this.Ok(this.Json(ex));
            }
        }

        /// <summary>
        /// Create User and Generate token.
        /// </summary>
        /// <returns>The user id and token</returns>
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetUserAndTokenAsync()
        {
            try
            {
                var response = await _acsManager.CreateUserAndTokenAsync();

                var jsonFormattedUser = new
                {
                    communicationUserId = response.User.Id
                };

                var clientResponse = new
                {
                    user = jsonFormattedUser,
                    token = response.AccessToken,
                    expiresOn = response.AccessToken.ExpiresOn
                };

                return this.Ok(clientResponse);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error occured while Generating Token: {ex}");

                return this.Ok(this.Json(ex));
            }
        }

        /// <summary>
        /// Refresh token for the specified user.
        /// </summary>
        /// <returns>The refreshed token.</returns>
        [Route("[action]/{identity}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetRefreshTokenAsync(string identity)
        {
            try
            {
                var response = await _acsManager.RefreshToken(identity);
                var clientResponse = new
                {
                    token = response.Token,
                    expiresOn = response.ExpiresOn
                };



                var id = SupportHelpers.FindIdFromToken(HttpContext.User.Claims);
                //Save new token in our own database
                var user = _accountManager.GetById(id);
                user.CommunicationToken = response.Token;
                ////TODO: add response check handling
                _accountManager.Update(id, user);
                //_accountManager.UpdateCommToken(id, response.Token);



                return this.Ok(clientResponse);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Error occured while Generating Token: {ex}");
                return this.Ok(this.Json(ex));
            }
        }
    }
}

