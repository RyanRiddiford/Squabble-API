using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Squabble.Managers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squabble.Models.RequestModels;
using Squabble.Models.ResponseModels;
using FriendRequest = Squabble.Models.FriendRequest;
using Squabble.Helpers;
using Microsoft.Extensions.Logging;

namespace Squabble.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        private readonly FriendRequestManager _friendRequestManager;
        private readonly ILogger<FriendRequestController> _logger;

        public FriendRequestController(FriendRequestManager friendRequestManager, ILogger<FriendRequestController> logger)
        {

            _friendRequestManager = friendRequestManager;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<FriendRequest> Get(int id) => await _friendRequestManager.Get(id);

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<FriendRequest>> GetFriendRequests(int id) => await _friendRequestManager.GetFriendRequests(id);

        /// <summary>
        /// Accept or deny a friend request
        /// </summary>
        [HttpPost]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AcceptFriendRequest([FromBody] AcceptFriendRequest requestModel)
        {

            var request = _friendRequestManager.GetById(requestModel.FriendRequestId);
            var user = SupportHelpers.FindIdFromToken(HttpContext.User.Claims);
            if (request == null || request.Accepted == true) {
                _logger.LogInformation($"User ID: {user} was somehow able to spam accept requests to User ID: {request.ReceiverID}.");
                return BadRequest();  
            }
            if (_friendRequestManager.CheckIfFriends(user, request.ReceiverID))
            {
                _logger.LogInformation($"User ID: {user} was somehow able to send a duplicate request to User ID: {request.ReceiverID}.");
                return BadRequest();
            }
            if (request.ReceiverID != user) {
                _logger.LogInformation($"User ID: {user} has been able to attempt to accept a friend request they should not be able to. Receiver of friend request: {request.ReceiverID}");
                return BadRequest(); 
            }
            await _friendRequestManager.AcceptFriendRequest(requestModel.FriendRequestId, requestModel.Accepted);
            return Ok();
        }

        /// <summary>
        /// Get all pending friend requests, both if the user is the sender or receiver.
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<PendingFriendRequestsResponse>> GetPendingFriendRequests()
        {
            var userId = SupportHelpers.FindIdFromToken(HttpContext.User.Claims);
            return await _friendRequestManager.GetPendingFriendRequests(userId);
        }

        /// <summary>
        /// Create a friend request between two users.
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] Models.RequestModels.FriendRequest requestModel)
        {
            var userId = SupportHelpers.FindIdFromToken(HttpContext.User.Claims);
            if (requestModel.FriendId == userId) {
                _logger.LogInformation($"User ID: {userId} was somehow able to send a request to themselves.");
                return BadRequest(); 
            }
            if (_friendRequestManager.CheckDoubleRequest(userId, requestModel.FriendId))
            {
                _logger.LogInformation($"User ID: {userId} was somehow able to send a duplicate request to User ID: {requestModel.FriendId}.");
                return BadRequest();
            }
            await _friendRequestManager.Add(userId, requestModel.FriendId);
            return Ok();
        }

        //deleting a request
        //default
        [HttpDelete("{id}")]
        public int? Delete(int id) => _friendRequestManager.Delete(id);
    }
}
