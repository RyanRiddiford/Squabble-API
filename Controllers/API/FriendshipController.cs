using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Squabble.Managers;
using Squabble.Models;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Security.Claims;
using Squabble.Helpers;

namespace Squabble.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {

        private readonly FriendshipManager _friendshipManager;

        public FriendshipController(FriendshipManager friendshipManager) => _friendshipManager = friendshipManager;


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<Friendship> Get(int id) => await _friendshipManager.Get(SupportHelpers.FindIdFromToken(HttpContext.User.Claims), id);

        [HttpGet]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<FriendInfo>> GetFriends()
        {
            var userId = SupportHelpers.FindIdFromToken(HttpContext.User.Claims);
            return await _friendshipManager.GetFriends(userId);
        }

        //when a new conversation is added
        [HttpPost]
        public async Task Post([FromBody] Friendship friend) => await _friendshipManager.Add(friend);


        //deleting a friend
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public int? Delete(int id) => _friendshipManager.Delete(SupportHelpers.FindIdFromToken(HttpContext.User.Claims), id);


    }
}
