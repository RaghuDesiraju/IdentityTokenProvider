using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenProvider.Models
{
    public interface ITokenProvider
    {
        Task<TokenResponse> GetToken(TokenRequest request);
    }
}
