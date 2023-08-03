using Squabble.Data;
using Squabble.Interfaces;
using Squabble.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Squabble.Models.Entities;
using Microsoft.Extensions.Logging;
using System;

namespace Squabble.Managers
{
    public class ChannelManager : IDataRepo<Channel, int>
    {
        private readonly SquabbleContext _context;
        private readonly ILogger<ChannelManager> _logger;

        public ChannelManager(SquabbleContext context, ILogger<ChannelManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        // try add channel and return the id, log errors and return null
        public async Task<int?> Add(Channel channel)
        {
            try
            {
                await _context.Channels.AddAsync(channel);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Error occurred with adding to database.");
                _logger.LogWarning(e.Message);
                _logger.LogWarning(e.StackTrace);
                return null;
            }
            return channel.ChannelId;
        }

        // get via channel id
        public async Task<Channel> Get(int id) => await _context.Channels.Where(x => x.ChannelId == id).FirstOrDefaultAsync();

        // get via thread id
        public async Task<Channel> GetByThreadId(string id) => await _context.Channels.Where(x => x.AzureChatThreadId == id).FirstOrDefaultAsync();


        public async Task<Channel> GetOneToOneChannel(int accountIdOne, int accountIdTwo)
        {
            Channel channel;
            try
            {
                channel = await _context.Channels
                .Where(x => x.ServerID == null && (x.ChannelName == $"{accountIdOne}-{accountIdTwo}" || x.ChannelName == $"{accountIdTwo}-{accountIdOne}"))
                .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Error occurred with getting from database.");
                _logger.LogWarning(e.Message);
                _logger.LogWarning(e.StackTrace);
                return null;
            }
            return channel;
        }

        // get users in a channel
        public Task<List<ChannelMember>> GetUsers(int id) => _context.ChannelMembers.Where(x => x.ChannelId == id).ToListAsync();

        public async Task<List<ChannelMemberInfo>> GetMemberAcsIds(int id, int accountId)
        {

            List<ChannelMemberInfo> channelMemberInfos = new List<ChannelMemberInfo>();

            List<User> users = new List<User>();

            var serverId = _context.Channels.Where(x => x.ChannelId == id).FirstOrDefault().ServerID;

            var admins = await _context.ServerAdmins.Where(x => x.ServerID == id).ToListAsync();
            string role = null;
            var owner = await _context.ServerOwners.Where(x => x.ServerID == serverId).FirstOrDefaultAsync();


            var members = await _context.ChannelMembers.Where(x => x.ChannelId == id).ToListAsync();

            foreach (var member in members)
            {
                users.Add(await _context.Accounts.Where(x => x.AccountId == member.UserID).FirstOrDefaultAsync());
            }

            foreach (var admin in admins)
            {
                users.Add(await _context.Accounts.Where(x => x.AccountId == admin.UserId).FirstOrDefaultAsync());
            }

            users.Add(await _context.Accounts.Where(x => x.AccountId == owner.UserId).FirstOrDefaultAsync());

            foreach (var ac in users)
            {

                role = null;

                foreach (var admin in admins)
                {
                    if (ac.AccountId == admin.UserId)
                    {
                        role = "Admin";
                    }
                }

                if (ac.AccountId == owner.UserId)
                {
                    role = "Owner";
                }
                else if (role == null)
                {
                    role = "Member";
                }

                channelMemberInfos.Add(
                new ChannelMemberInfo
                {
                    ACSId = ac.CommunicationUserId,
                    FirstName = ac.FirstName,
                    MiddleName = ac.MiddleName,
                    Surname = ac.Surname,
                    UserName = ac.UserName,
                    Role = role
                });
            }
            return channelMemberInfos;
        }

        // delete a channel with supplied id
        public int? Delete(int? id)
        {
            var chan = _context.Channels.FirstOrDefault(x => x.ChannelId == id);
            if (chan != null)
            {
                _context.Channels.Remove(chan);
            }

            _context.SaveChanges();

            return id;
        }

        // add a member, return an error if they are already here or aren't in the server hosting the channel
        public async Task<int?> AddUser(ChannelMember member)
        {
            if (_context.ChannelMembers.Any(x => x.ChannelId == member.ChannelId && x.UserID == member.UserID)) { _logger.LogInformation($"{member.UserID} already exists in {member.ChannelId}"); return null; }
            var serv = await _context.Channels.Where(x => x.ChannelId == member.ChannelId).FirstOrDefaultAsync();
            if (!await _context.ServerMembers.AnyAsync(x => x.ServerID == serv.ServerID && x.UserID == member.UserID))
            {
                _logger.LogError($"User: {member.UserID} - Server: {serv.ServerID} - Channel: {serv.ChannelName}.");
                _logger.LogError("A user was somehow able to be added to a channel in a server they aren't apart of.");
                return null;
            }
            try
            {
                await _context.ChannelMembers.AddAsync(member);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Error occurred with adding to database.");
                _logger.LogWarning(e.Message);
                _logger.LogWarning(e.StackTrace);
                return null;
            }

            return member.UserID;
        }

        // remove a user from the channel
        public async Task<int?> RemoveUser(ChannelMember member)
        {
            try
            {
                _context.ChannelMembers.Remove(member);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning("Error occurred with removing from database.");
                _logger.LogWarning(e.Message);
                _logger.LogWarning(e.StackTrace);
                return null;
            }

            return member.UserID;
        }
        //return channel by id
        public Channel GetById(int id)
        {
            return _context.Channels.Where(x => x.ChannelId == id).FirstOrDefault();
        }
    }
}