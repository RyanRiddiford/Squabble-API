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
    public class ServerController : ControllerBase
    {
        private readonly ServerManager _serverManager;
        private readonly ILogger<ServerController> _logger;

        public ServerController(ServerManager serverManager, ILogger<ServerController> logger)
        {
            _serverManager = serverManager;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<Server> Get(int id) => await _serverManager.Get(id);

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<ServerMember>> GetUsers(int id) => await _serverManager.GetUsers(id);

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ServerOwner> GetOwner(int id) => await _serverManager.GetOwner(id);

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<ServerAdmin>> GetAdmins(int id) => await _serverManager.GetAdmins(id);

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public List<Server> GetServers(int id) => _serverManager.GetServers(SupportHelpers.FindIdFromToken(HttpContext.User.Claims));

        [HttpGet]
        [Route("[action]/{serverId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public List<Channel> GetChannels(int serverId)
        {
            return _serverManager.GetChannels(serverId, SupportHelpers.FindIdFromToken(HttpContext.User.Claims));
        }

        [Route("[action]")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task AddUser([FromBody] ServerMember member)
        {
            if (member == null) { _logger.LogError("Null member sent here."); return; }
            await _serverManager.AddUser(member);
        }


        /// <summary>
        /// Create a new server.
        /// </summary>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task PostAsync([FromBody] CreateServerRequest requestModel)
        {
            if (requestModel == null || string.IsNullOrEmpty(requestModel.Name)) { _logger.LogError("Null server sent here."); return; }
            var owner = new ServerOwner
            {
                UserId = SupportHelpers.FindIdFromToken(HttpContext.User.Claims)
            };

            var server = new Server
            {
                ServerName = requestModel.Name,
                ServerOwner = owner
            };

            await _serverManager.Add(server);
        }

        //deleting a server
        //default
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public int Delete(int id) => _serverManager.Delete(id);

        /// <summary>
        /// Replace an owner of a server with another. Only an admin can become an owner and the
        /// current owner goes back to becoming an admin.
        /// </summary>
        [HttpPost]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ReplaceOwner([FromBody] UpdateServerUserRoleRequest requestModel)
        {
            if (requestModel == null) { _logger.LogError("Null owner sent here."); return BadRequest(); }
            await _serverManager.ReplaceOwner(requestModel.ServerId, SupportHelpers.FindIdFromToken(HttpContext.User.Claims), requestModel.AccountId);
            return Ok();
        }

        /// <summary>
        /// Promote a member to an admin.
        /// </summary>
        [HttpPost]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> MakeAdmin([FromBody] UpdateServerUserRoleRequest requestModel)
        {
            if (requestModel == null) { _logger.LogError("Null admin sent here."); return BadRequest(); }
            await _serverManager.MakeAdmin(requestModel.ServerId, requestModel.AccountId);
            return Ok();
        }

        /// <summary>
        /// Demote an admin to a member.
        /// </summary>
        [HttpPost]
        [Route("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> MakeMember([FromBody] UpdateServerUserRoleRequest requestModel)
        {
            if (requestModel == null) { _logger.LogError("Null member sent here."); return BadRequest(); }
            await _serverManager.MakeMember(requestModel.ServerId, requestModel.AccountId);
            return Ok();
        }
    }
}
