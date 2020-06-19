using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityTokenProvider.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTokenProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenProvider _tokenProvider;

        public TokenController(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        [HttpPost]
        public async Task<ActionResult<TokenResponse>> Post(TokenRequest request)//[FromForm] 
        {
            
            var response = await _tokenProvider.GetToken(request);

            if (!string.IsNullOrEmpty(response.Error))
            {
                return new BadRequestObjectResult(response);
            }

            return response;
        }


    }
}