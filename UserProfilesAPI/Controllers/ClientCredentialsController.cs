using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UserProfilesAPI.Data;
using UserProfilesAPI.Dto;
using UserProfilesAPI.Model;

namespace UserProfilesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientCredentialsController : ControllerBase
    {
        private readonly ICredentialRepository _repo;
        private readonly IConfiguration _config = null;


        public ClientCredentialsController(ICredentialRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(ClientCredentialDto clientCredentials)//string userName, string password)[FromBody]
        {
            if (clientCredentials == null)
                return BadRequest("Client request not sent");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _repo.ClientIdExists(clientCredentials.ClientID))
            {
                return BadRequest("Client Id already exists");
            }
            var createdCredential = await _repo.Register(clientCredentials);//http://example.org/myitem
         
            if(createdCredential == null)
                return Ok(clientCredentials.ClientID + " has not been created");
            else          
                return Created("http://FOSObjects.org/myitem", new { message = clientCredentials.Id + " has been created" });

        }

        [HttpGet("GetAllCredentials")]
        public async Task<IActionResult>GetAllCredentials()
        {
            
            var credentials = await _repo.GetAllClientCredentials();
            return Ok(credentials);
        }
    }
}