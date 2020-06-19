using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenProvider.Dto
{
    public class DTOObj
    {
        public List<ClientCredentialDto> ClientCredentialDtoList { get; set; }
    }
    public class ClientCredentialDto
    {      
        public string ClientID { get; set; }
        //SHA 256 Format     
        public string ClientSecret { get; set; }

        public string AllowedGrantTypes { get; set; }
        public int? AllowOfflineAccess { get; set; }

        //[JsonConverter(typeof(StringEnumConverter))]
        public TokenUsage RefreshTokenUsage { get; set; }
        public int AccessTokenLifetime { get; set; }

        //[JsonConverter(typeof(StringEnumConverter))]
        public TokenExpiration RefreshTokenExpiration { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
    }
}
