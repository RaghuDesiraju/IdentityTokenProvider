using IdentityTokenProvider.Dto;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityTokenProvider.Models
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {            
            UserDto user = ClientStore.GetUser(context.Request.Raw["username"],
                context.Request.Raw["password"]);
          

            if (user == null)
            {
                //return Task.CompletedTask;
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                       "Incorrect username/password");
            }

            context.Result.IsError = false;
            context.Result.Subject = GetClaimsPrincipal();

            return Task.CompletedTask;
        }

        private static ClaimsPrincipal GetClaimsPrincipal()
        {
            var issued = DateTimeOffset.Now.ToUnixTimeSeconds();

            var claims = new List<Claim>
            {
              new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()),
              new Claim(JwtClaimTypes.AuthenticationTime, issued.ToString()),
              new Claim(JwtClaimTypes.IdentityProvider, "localhost")
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }
    }
}
