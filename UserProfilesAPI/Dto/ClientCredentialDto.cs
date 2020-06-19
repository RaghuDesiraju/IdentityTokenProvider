using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserProfilesAPI.Dto
{
    public class ClientCredentialDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 16)]
        public string ClientID { get; set; }
        ////SHA 256 Format
        [Required]
        [StringLength(64, MinimumLength = 12, ErrorMessage = "You must specify secret between 12 and 64 characters")]
        public string ClientSecret { get; set; }

        //public byte[] ClientSecretHash { get; set; }
        //public byte[] ClientSecretSalt { get; set; }


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
