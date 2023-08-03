using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Squabble.Managers;
using Squabble.Models;
using System.Collections.Generic;
using Squabble.Helpers;
using Microsoft.Extensions.Logging;

namespace Squabble.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class KanbanController : ControllerBase
    {

        private readonly KanbanManager _kanbanManager;
        private readonly ILogger<KanbanController> _logger;

        public KanbanController(KanbanManager kanbanManager, ILogger<KanbanController> logger)
        {
            _kanbanManager = kanbanManager;
            _logger = logger;
        }

        //return all kanbans for a user, sorted by ascending
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<KanbanItem> Get()
        {
            return _kanbanManager.GetAll(SupportHelpers.FindIdFromToken(HttpContext.User.Claims));
        }

        //Returns the kanban - redundant
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public KanbanItem Get(int id)
        {
            return _kanbanManager.GetById(id);
        }

        //edit kanban
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public void Put([FromBody] KanbanItem item)
        {
            if (item == null)
            {
                _logger.LogWarning("Null item has been received.");
                return;
            }
            _kanbanManager.Update(item.KanbanItemID, item);
        }

        //new item
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async void Post([FromBody] KanbanItem item)
        {
            if(item == null)
            {
                _logger.LogWarning("Null item has been received.");
                return;
            }
            await _kanbanManager.Add(item);
        }

        //user removes a kanban item
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public int Delete(int id)
        {
            return _kanbanManager.Delete(id);
        }
    }
}
