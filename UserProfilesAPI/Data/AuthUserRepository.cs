using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserProfilesAPI.Dto;
using UserProfilesAPI.Model;

namespace UserProfilesAPI.Data
{
    public class AuthUserRepository:IAuthUserRepository
    {
        private AuthDbContext _context = null;
        public AuthUserRepository(AuthDbContext context)
        {
            _context = context;
        }

        private bool CompareStrings(string sourceString, string targetString)
        {
            return String.Equals(sourceString, targetString,
                                StringComparison.InvariantCultureIgnoreCase);
        }
        public async Task<AuthUserDto> RegisterUser(AuthUserDto user)
        {
            var userrep = await _context.AuthUser.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            string[] roleSplit = user.RoleName.Split(new char[] { ',' });

            foreach(string roleVal in roleSplit)
            {
                if (_context.UserRole != null)
                {
                    //cannit use string.compare
                    var role = await _context.UserRole.FirstOrDefaultAsync(
                                x => !string.IsNullOrWhiteSpace(roleVal) && 
                                !string.IsNullOrWhiteSpace(x.RoleName) && 
                                roleVal.ToLower()==x.RoleName.ToLower());
                    if (role == null)
                    {
                        role = new Role();
                        role.RoleName = user.RoleName;
                        await _context.UserRole.AddAsync(role);
                        await _context.SaveChangesAsync();
                    }
                }

            }
            
            if (userrep == null)
                userrep = new AuthUser();

            userrep.UserName = user.UserName;
            userrep.FirstName = user.FirstName;
            userrep.LastName = user.LastName;
            userrep.Email = user.Email;
            userrep.Address = user.Address;
            userrep.UserandRole.Clear();

                       
            userrep.UserandRole = new List<UserandRole>();

            foreach (string roleVal in roleSplit)
            {
                UserandRole userRole = new UserandRole();
                /*var role = await _context.UserRole.FirstOrDefaultAsync(
                    x => String.Equals(x.RoleName, roleVal,
                    StringComparison.OrdinalIgnoreCase));*/
                var role = await _context.UserRole.FirstOrDefaultAsync(
                                              x => !string.IsNullOrWhiteSpace(roleVal) &&
                                              !string.IsNullOrWhiteSpace(x.RoleName) &&
                                              roleVal.ToLower() == x.RoleName.ToLower());
                userRole.UserRole = role;
                userRole.RoleId = role.RoleId;                
                userRole.AuthUser = userrep;
                userRole.AuthUserId = userrep.AuthUserId;
                userrep.UserandRole.Add(userRole);
            }          
            
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);
            userrep.PasswordHash = passwordHash;
            userrep.PasswordSalt = passwordSalt;
            await _context.AuthUser.AddAsync(userrep);
            await _context.SaveChangesAsync();
            return user;
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }

        public static string GetPasswordEncoded(byte[] passwordHash)
        {
            return System.Text.Encoding.UTF8.GetString(passwordHash);
        }


        public async Task<bool> UserExists(string userName)
        {
            var user = await _context.AuthUser.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
                return false;
            else
                return true;
        }

        public async Task<AuthUserDto> LoginUser(AuthUserDto user)
        {
            var userVal = await _context.AuthUser.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if (user == null)
                return null;
            
            if (!VerifyPasswordHash(user.Password, userVal.PasswordHash, userVal.PasswordSalt))
            {
                return null;
            }
            return user;

        }


    }
}
