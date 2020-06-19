using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserProfilesAPI.Dto;
using UserProfilesAPI.Model;

namespace UserProfilesAPI.Data
{
    public interface ICredentialRepository
    {
        Task<ClientCredentialDto> Register(ClientCredentialDto credential);
        Task<IEnumerable<ClientCredentialDto>> GetAllClientCredentials();
        Task<bool> ClientIdExists(string clientID);
    }
}
