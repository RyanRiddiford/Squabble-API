using Squabble.Data;
using Squabble.Interfaces;
using Squabble.Models;
using System.Threading.Tasks;
using System.Linq;
using Squabble.Models.RequestModels;
using Microsoft.Extensions.Logging;
using System;

namespace Squabble.Managers
{
    public class LoginManager : IDataRepo<Login, int>
    {

        private readonly SquabbleContext _context;
        private readonly ILogger<LoginManager> _logger;

        public LoginManager(SquabbleContext context, ILogger<LoginManager> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Save a Login to the database.
        /// </summary>
        public async Task Add(Login login)
        {
            try
            {
                await _context.Logins.AddAsync(login);
                await _context.SaveChangesAsync();
            } catch(Exception e)
            {
                _logger.LogError($"{login.AccountId} failed to be added to the database.");
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return;
            }
            
        }

        public async Task<string> CreateAsync(Login login)
        {
            try
            {
                await _context.Logins.AddAsync(login);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"{login.AccountId} failed to be added to the database.");
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return null;
            }

            return login.UserName;
        }

        public async Task<int?> VerifyCredentials(LoginRequest login)
        {

            Login result;

            if (login.Email == null)
            {
                result = _context.Logins.Where(x => x.UserName == login.UserName).FirstOrDefault();
            }
            else
            {
                result = await _context.Logins.FindAsync(login.Email);
            }

            if (result != null)
            {
                if (SimpleHashing.PBKDF2.Verify(result.PasswordHash, login.Password))
                {
                    return _context.Accounts.Where(x => x.UserName == login.UserName).First().AccountId;
                }
                else
                {
                    _logger.LogInformation($"UN: {result.UserName} | EM: {result.Email} has failed their login.");
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a Login object with the matching ID if it exists.
        /// </summary>
        public Login GetById(int id)
        {
            var login = _context.Logins.FirstOrDefault(x => x.AccountId == id);
            if (login == null) { return null; }
            return login;
        }

        /// <summary>
        /// Returns a Login object with the matching email or username if it exists.
        /// </summary>
        public Login GetByEmailOrUsername(string id)
        {
            var login = _context.Logins.FirstOrDefault(x => x.Email == id || x.UserName == id);
            if (login == null) { return null; }
            return login;
        }
    }
}
