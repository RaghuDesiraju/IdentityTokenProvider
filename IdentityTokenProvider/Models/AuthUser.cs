using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenProvider.Models
{
    /// <summary>
    /// User Credentials. Not required if the grant type is client_credentials
    /// </summary>
    public class AuthUser
    {
        public AuthUser()
        {
            this.UserandRole = new HashSet<UserandRole>();
        }
        public int AuthUserId { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "You must specify secret between 6 and 10 characters")]
        public string UserName { get; set; }
        //public string Password { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<UserandRole> UserandRole { get; set; }

    }
}
