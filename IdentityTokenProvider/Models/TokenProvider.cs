using IdentityModel;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using IdpTokenResponse = IdentityServer4.ResponseHandling.TokenResponse;
namespace IdentityTokenProvider.Models
{
    public class TokenProvider : ITokenProvider
    {
        private readonly ITokenRequestValidator _requestValidator;
        private readonly IClientSecretValidator _clientValidator;
        private readonly ITokenResponseGenerator _responseGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenProvider(
          ITokenRequestValidator requestValidator,
          IClientSecretValidator clientValidator,
          ITokenResponseGenerator responseGenerator,
          IHttpContextAccessor httpContextAccessor)
        {
            _requestValidator = requestValidator;
            _clientValidator = clientValidator;
            _responseGenerator = responseGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenResponse> GetToken(TokenRequest request)
        {
            var parameters = new NameValueCollection
              {
                { "username", request.Username },
                { "password", request.Password },
                { "grant_type", request.GrantType },
                { "scope", request.Scope },
                { "refresh_token", request.RefreshToken },
                { "response_type", OidcConstants.ResponseTypes.Token }
              };

            var response = await GetIdpToken(parameters, request);
            
            return GetTokenResponse(response);
        }

        private async Task<IdpTokenResponse> GetIdpToken(NameValueCollection parameters, TokenRequest request)
        {
            
            var clientResult = await _clientValidator.ValidateAsync(_httpContextAccessor.HttpContext);
            string clientString = "";
            foreach(var client in ClientStore.GetClients())
            {
                clientString += client.ClientName + ": " + client.ClientSecrets + Environment.NewLine;
            }

            string query = "";
            foreach(var qry in _httpContextAccessor.HttpContext.Request.Query)
            {
                query += qry.Key + ": " + qry.Value + " ";
            }

            if (clientResult.IsError)
            {
                return new IdpTokenResponse
                {
                    Custom = new Dictionary<string, object>
                    {
                        { "Error", "invalid_client " + request.Username + "query: " + query
                        + " " + " client result " + clientResult.Error + clientResult.ErrorDescription },
                        { "ErrorDescription", "Invalid client/secret combination " }//+ request.Username + " clienstore: " + clientString
                    }
                };
            }

            var validationResult = await _requestValidator.ValidateRequestAsync(parameters, clientResult);

            if (validationResult.IsError)
            {
                return new IdpTokenResponse
                {
                    Custom = new Dictionary<string, object>
                    {
                        { "Error", validationResult.Error },
                        { "ErrorDescription", validationResult.ErrorDescription }
                    }
                };
            }

            return await _responseGenerator.ProcessAsync(validationResult);
        }

        private static TokenResponse GetTokenResponse(IdpTokenResponse response)
        {
            if (response.Custom != null && response.Custom.ContainsKey("Error"))
            {
                return new TokenResponse
                {
                    Error = response.Custom["Error"].ToString(),
                    ErrorDescription = response.Custom["ErrorDescription"]?.ToString()
                };
            }

            return new TokenResponse
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                ExpiresIn = response.AccessTokenLifetime,
                TokenType = "Bearer"
            };
        }
    }
}
