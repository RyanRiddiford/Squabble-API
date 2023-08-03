using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Squabble.Data;
using Squabble.Interfaces;
using Squabble.Models;

namespace Squabble.Managers
{
    public class ServerManager : IDataRepo<Server, int>
    {
        private readonly SquabbleContext _context;
        private readonly ILogger<ServerManager> _logger;

        public ServerManager(SquabbleContext context, ILogger<ServerManager> logger)
        {
            _context = context;
            _logger = logger;
        }
        //redundant to fulfill interface requirements
        public Server GetById(int id)
        {
            return _context.Servers.FirstOrDefault(x => x.ServerID == id);

        }
        //returns post with id.. redundant, just to
        public async Task<Server> Get(int id)
        {
            return await _context.Servers.FirstOrDefaultAsync(x => x.ServerID == id);

        }
        //get users for a specific server
        public async Task<List<ServerMember>> GetUsers(int id)
        {
            return await _context.ServerMembers
                .Include(x => x.User)
                .Where(x => x.ServerID == id).ToListAsync();
        }
        //get admins for a specific server
        public async Task<List<ServerAdmin>> GetAdmins(int id)
        {
            return await _context.ServerAdmins
                .Include(x => x.User)
                .Where(x => x.ServerID == id).ToListAsync();
        }
        //get owner for a specific server
        public async Task<ServerOwner> GetOwner(int id)
        {
            return await _context.ServerOwners
                .Include(x => x.User)
                .Where(x => x.ServerID == id).FirstOrDefaultAsync();

            // TODO: Only return a cut down user object like we do for friends.
        }

        //get servers for specific user
        public List<Server> GetServers(int id)
        {
            return _context.Servers
            .Where(c => c.Members.Any(m => m.UserID == id) ||
                                                   c.Admins.Any(x => x.UserId == id) ||
                                                   c.ServerOwner.UserId == id)
            .ToList();
        }

        public async Task<int?> Add(Server server)
        {
            if (server == null) { _logger.LogError("Null server sent here."); return null; }
            try
            {
                await _context.Servers.AddAsync(server);
                await _context.SaveChangesAsync();
            } catch (Exception e)
            {
                _logger.LogWarning("Error occurred with adding server to database.");
                _logger.LogWarning(e.Message);
                _logger.LogWarning(e.StackTrace);
                return null;
            }
            

            return server.ServerID;
        }

        public async Task<int?> AddUser(ServerMember member)
        {
            if (member == null) { _logger.LogError("Null member sent here."); return null; }
            if (_context.ServerMembers.Any(x => x.ServerID == member.ServerID && x.UserID == member.UserID)) { _logger.LogInformation($"User {member.UserID} already exists in server {member.ServerID}"); return null; }
            try
            {
                await _context.ServerMembers.AddAsync(member);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Error occurred with adding member to database.");
                _logger.LogWarning(e.Message);
                _logger.LogWarning(e.StackTrace);
                return null;
            }
           

            return member.UserID;
        }
        public async Task<int?> RemoveUser(int id, int userID)
        {
            try
            {
                _context.ServerMembers.Remove(await _context.ServerMembers.FirstOrDefaultAsync(x => x.ServerID == id && x.UserID == userID));
                foreach (var chanM in _context.ChannelMembers.Where(x => x.Channel.ServerID == id && x.UserID == userID).ToList())
                {
                    _context.ChannelMembers.Remove(chanM);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Error occurred with removing users from server.");
                _logger.LogWarning(e.Message);
                _logger.LogWarning(e.StackTrace);
                return null;
            }
            

            return userID;
        }

        public int Delete(int id)
        {
            try
            {
                _context.Servers.Remove(_context.Servers.Find(id));
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Error occurred with removing servers from database.");
                _logger.LogWarning(e.Message);
                _logger.LogWarning(e.StackTrace);
                return -1;
            }
            

            return id;
        }

        public List<Channel> GetChannels(int serverId, int userId)
        {
            return _context.Channels
                .Where(c => c.ServerID == serverId && (c.Members.Any(m => m.UserID == userId) ||
                                                       c.Server.ServerOwner.UserId == userId ||
                                                       c.Server.Admins.Any(sa => sa.UserId == userId)))
                .ToList();
        }

        public async Task ReplaceOwner(int serverId, int oldOwnerId, int newOwnerId)
        {
            if (_context.ServerOwners.Any(x => x.ServerID == serverId && x.UserId == newOwnerId)) { _logger.LogInformation($"Owner {newOwnerId} already exists in server {serverId}"); return; }
            try
            {
                // Add new owner.
                _context.ServerOwners.Add(
                    new ServerOwner
                    {
                        ServerID = serverId,
                        UserId = newOwnerId
                    }
                );

                // Remove old owner.
                var currentOwner = await _context.ServerOwners.FirstOrDefaultAsync(x => x.UserId == oldOwnerId && x.ServerID == serverId);
                _context.ServerOwners.Remove(currentOwner);

                // Make old owner an admin.
                _context.ServerAdmins.Add(
                    new ServerAdmin
                    {
                        UserId = oldOwnerId,
                        ServerID = serverId
                    }
                );

                // Remove old admin.
                var oldAdmin = await _context.ServerAdmins.FirstOrDefaultAsync(x => x.UserId == newOwnerId && x.ServerID == serverId);
                _context.ServerAdmins.Remove(oldAdmin);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Error occurred with replacing owner.");
                _logger.LogWarning(e.Message);
                _logger.LogWarning(e.StackTrace);
                return;
            }

            
        }

        public async Task MakeAdmin(int serverId, int accountId)
        {
            if (_context.ServerOwners.Any(x => x.ServerID == serverId && x.UserId == accountId)) { _logger.LogInformation($"Admin {accountId} already exists in server {serverId}"); return; }
            try
            {
                // Add new admin access.
                _context.ServerAdmins.Add(
                    new ServerAdmin
                    {
                        ServerID = serverId,
                        UserId = accountId
                    }
                );

                // Remove old member access.
                var currentMember = await _context.ServerMembers.FirstOrDefaultAsync(x => x.UserID == accountId && x.ServerID == serverId);
                _context.ServerMembers.Remove(currentMember);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning($"User {accountId} on {serverId} - Error occurred with adding admin access to user.");
                _logger.LogWarning(e.Message);
                _logger.LogWarning(e.StackTrace);
                return;
            }
            
        }

        public async Task MakeMember(int serverId, int accountId)
        {
            if (_context.ServerMembers.Any(x => x.ServerID == serverId && x.UserID == accountId)) { _logger.LogInformation($"Member {accountId} already exists in server {serverId}"); return; }
            try
            {
                // Add new admin access.
                _context.ServerMembers.Add(
                    new ServerMember
                    {
                        ServerID = serverId,
                        UserID = accountId
                    }
                );

                // Remove old admin access.
                var currentAdmin = await _context.ServerAdmins.FirstOrDefaultAsync(x => x.UserId == accountId && x.ServerID == serverId);
                _context.ServerAdmins.Remove(currentAdmin);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning($"User {accountId} on {serverId} - Error occurred with removing admin access from user.");
                _logger.LogWarning(e.Message);
                _logger.LogWarning(e.StackTrace);
                return;
            }
          
        }
    }
}
