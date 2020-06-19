using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenProvider.Models
{
    public class TokenRequest
    {
        [FromForm(Name = "username")]
        public string Username { get; set; }
        [FromForm(Name = "password")]
        public string Password { get; set; }
        [FromForm(Name = "grant_type")]
        public string GrantType { get; set; }
        [FromForm(Name = "scope")]
        public string Scope { get; set; }
        [FromForm(Name = "refresh_token")]
        public string RefreshToken { get; set; }
    }
}
