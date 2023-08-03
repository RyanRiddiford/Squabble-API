using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Squabble.Helpers;
using Squabble.Managers;
using Squabble.Models;
using Squabble.Models.RequestModels;

namespace Squabble.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {

        private readonly ChannelManager _channelManager;
        private readonly ILogger<ChannelController> _logger;
        public ChannelController(ChannelManager channelManager, ILogger<ChannelController> logger)
        {
            _channelManager = channelManager;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<Channel> Get(int id) => await _channelManager.Get(id);

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<Channel> GetByThreadId(string id)
        {
            return await _channelManager.GetByThreadId(id);
        }

        [HttpGet]
        [Route("[action]/{friendId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<Channel> GetOneToOneChannel(int friendId)
        {
            var userId = SupportHelpers.FindIdFromToken(HttpContext.User.Claims);
            return await _channelManager.GetOneToOneChannel(userId, friendId);
        }



        [HttpGet]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<ChannelMember>> GetUsers(int id) => await _channelManager.GetUsers(id);

        [HttpPost]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddUser([FromBody] ChannelMember member)
        {
            if (member == null)
            {
                _logger.LogError("Null member appeared here!");
                return BadRequest("Null member");
            }
            if (await _channelManager.AddUser(member) == null)
            {
                return BadRequest("User is not in the server.");
            }
            return Ok();
        }

        /// <summary>
        /// Add a new chanel.
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<int> Post([FromBody] CreateChannelRequest createChannelRequest)
        {
            if (createChannelRequest == null) { _logger.LogError("Null request appeared here!"); return -1; }
            var channel = new Channel
            {
                AzureChatThreadId = createChannelRequest.AzureChatThreadId,
                ChannelName = createChannelRequest.ChannelName,
                ServerID = createChannelRequest.ServerId
            };

            await _channelManager.Add(channel);

            var userId = SupportHelpers.FindIdFromToken(HttpContext.User.Claims);

            var channelMember = new ChannelMember
            {
                ChannelId = channel.ChannelId,
                UserID = userId
            };

            await _channelManager.AddUser(channelMember);

            return channel.ChannelId;
        }

        //deleting a chann, for whatever reason
        //default
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public int? Delete(int id) => _channelManager.Delete(id);


        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<ChannelMemberInfo>> GetMemberAcsIds(int id) => await _channelManager.GetMemberAcsIds(id, SupportHelpers.FindIdFromToken(HttpContext.User.Claims));


        [HttpPost]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task RemoveUser([FromBody] ChannelMember member)
        {
            if (member == null) { _logger.LogError("Null request appeared here!"); return; }
            await _channelManager.RemoveUser(member);
        }
    }
}