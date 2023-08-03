using Azure.Communication;
using Azure.Communication.Identity;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Squabble.Interfaces
{
    public interface ICommunicationTokenManager
    {
        public Task<CommunicationUserIdentifier> CreateUserAsync();
        public Task<AccessToken> CreateTokenAsync(CommunicationUserIdentifier identity);
        public Task<CommunicationUserIdentifierAndToken> CreateUserAndTokenAsync();
        public Task<AccessToken> RefreshToken(string id);
    }
}
