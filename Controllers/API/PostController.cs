using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Squabble.Managers;
using Squabble.Models;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Squabble.Models.Entities;
using Microsoft.Extensions.Logging;

namespace Squabble.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly PostManager _postManager;
        private readonly ILogger<PostController> _logger;
        public PostController(PostManager postManager, ILogger<PostController> logger)
        {
            _postManager = postManager;
            _logger = logger;
        }

        //default, won't really be used
        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return _postManager.GetAll();
        }

        //Returns the post
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Post Get(int id)
        {
            return _postManager.Get(id);
        }

        //edit post
        //eg. if a user wants to edit their post, this will be called
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public void Put([FromBody] Post post)
        {
            if (post == null) { _logger.LogError("Null post received here."); return; }
            _postManager.Update(post.PostId, post);
        }

        //new post
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task Post([FromBody] Post post)
        {
            if (post == null) { _logger.LogError("Null post received here."); return; }
            await _postManager.Add(post);
        }

        //user or admin removes a post
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public int Delete(int id)
        {
            return _postManager.Delete(id);
        }
    }
}
