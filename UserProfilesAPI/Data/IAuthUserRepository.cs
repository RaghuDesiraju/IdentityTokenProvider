using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserProfilesAPI.Dto;

namespace UserProfilesAPI.Data
{
    public interface IAuthUserRepository
    {
        Task<AuthUserDto> RegisterUser(AuthUserDto user);
        Task<AuthUserDto> LoginUser(AuthUserDto user);
    }
}
