using Azure.Communication;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Azure.Communication.Identity;
using Squabble.Interfaces;

namespace Squabble.Managers
{
    public class CommunicationTokenManager : ICommunicationTokenManager
    {
        public CommunicationIdentityClient Client { get; private set; }

        public CommunicationTokenManager(IConfiguration config)
        {
            Client = new CommunicationIdentityClient(config.GetConnectionString("ACSTextChat"));
        }

        /// <summary>
        /// Creates a new communication user.
        /// </summary>
        /// <returns>The new user's id.</returns>
        public async Task<CommunicationUserIdentifier> CreateUserAsync()
        {
            var identityResponse = await Client.CreateUserAsync();
            var identity = identityResponse.Value;

            return identity;
        }
        /// <summary>
        /// Creates an access token for Azure Communication Services.
        /// </summary>
        /// <param name="identity">The identity of the communication user.</param>
        /// <returns>The new access token.</returns>
        public async Task<AccessToken> CreateTokenAsync(CommunicationUserIdentifier identity)
        {
            var tokenResponse = await Client.GetTokenAsync(identity, scopes: new[] { CommunicationTokenScope.Chat, CommunicationTokenScope.VoIP });

            return tokenResponse.Value;
        }
        /// <summary>
        /// Creates a user and communication access token
        /// </summary>
        /// <returns>The user and token.</returns>
        public async Task<CommunicationUserIdentifierAndToken> CreateUserAndTokenAsync()
        {
            var response = await Client.CreateUserAndTokenAsync(scopes: new[] { CommunicationTokenScope.Chat, CommunicationTokenScope.VoIP });

            return response.Value;
        }
        /// <summary>
        /// Refresh the access token for the specified user id.
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <returns>The refreshed token.</returns>
        public async Task<AccessToken> RefreshToken(string id)
        {
            var identityToRefresh = new CommunicationUserIdentifier(id);
            var tokenResponse = await Client.GetTokenAsync(identityToRefresh, scopes: new[] { CommunicationTokenScope.Chat, CommunicationTokenScope.VoIP });

            return tokenResponse.Value;
        }


    }
}
