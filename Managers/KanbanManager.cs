using Squabble.Data;
using Squabble.Interfaces;
using Squabble.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace Squabble.Managers
{
    public class KanbanManager : IDataRepo<KanbanItem, int>
    {
        private readonly SquabbleContext _context;
        private readonly ILogger<KanbanManager> _logger;

        public KanbanManager(SquabbleContext context, ILogger<KanbanManager> logger)
        {
            _context = context;
            _logger = logger;
        }
        //returns items... default implementation
        public IEnumerable<KanbanItem> GetAll(int id)
        {
            List<KanbanItem> items = new();
            try
            {
                items = _context.KanbanItems.Where(x => x.UserID == id).OrderBy(x => x.Position).ToList();
            } catch(Exception e)
            {
                _logger.LogWarning("Could not return items.");
            }
            return items;
        }

        //returns post with id.. redundant, default method
        public KanbanItem GetById(int id)
        {
            KanbanItem item = new();
            try
            {
                item = _context.KanbanItems.Where(x => x.KanbanItemID == id).FirstOrDefault();
            } catch(Exception e)
            {
                _logger.LogWarning("Could not find item.");
            }
            return item;

        }

        public async Task<int?> Add(KanbanItem item)
        {
            try
            {
                await _context.KanbanItems.AddAsync(item);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Could not add item.");
            }
         
            return item.KanbanItemID;
        }
        //throw request here, update and save then return the id
        public int Update(int id, KanbanItem item)
        {
            if (!_context.KanbanItems.Any(x => x.KanbanItemID == id))
            {
                return -1;
            }
            try
            {
                _context.Update(item);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Could not update item.");
            }

            return id;
        }

        public int Delete(int id)
        {
            try
            {
                _context.KanbanItems.Remove(_context.KanbanItems.Find(id));
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Could not delete item.");
            }

            return id;
        }
    }
}
