using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserProfilesAPI.Data;
using UserProfilesAPI.Dto;

namespace UserProfilesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IAuthUserRepository _repo;
        private readonly IConfiguration _config = null;


        //public AuthUserController()
        //{

        //}
        public AuthUserController(IAuthUserRepository repo, IConfiguration mapper)
        {
            _repo = repo;
            _config = mapper;
        }

       
        [HttpPost("SaveUser")]
        public async Task<IActionResult> SaveUser(AuthUserDto user)
        {
            var userVal = await _repo.RegisterUser(user);
            return (Ok("User " + user.UserName + " created/updated"));

        }

        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser(AuthUserDto user)
        {
            var userVal = await _repo.LoginUser(user);
            return Ok(user);

        }

    }
}