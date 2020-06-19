using IdentityServer4.Models;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserProfilesAPI.Model
{
    /// <summary>
    /// Client credentials with all information about the client
    /// </summary>
    public class ClientCredentials
    {
        public int Id { get; set; }

        //[Required]
        //[StringLength(64, MinimumLength = 16)]
        public string ClientID { get; set; }
        ////SHA 256 Format
        //[Required]
        //[StringLength(128, MinimumLength = 24, ErrorMessage = "You must specify secret between 8 and 15 characters")]
        //public string ClientSecret { get; set; }

        //[MaxLength]
        public string ClientSecretHash { get; set; }
                                                                                                                                                                                                                                                                                
        //[MaxLength]
        //public string ClientSecretSalt { get; set; }


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
