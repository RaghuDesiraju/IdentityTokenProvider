using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTokenProvider.Models
{
    public class Role
    {
        public Role()
        {
            this.UserandRole = new HashSet<UserandRole>();
        }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public virtual ICollection<UserandRole> UserandRole { get; set; }
    }

    public class UserandRole
    {
        public int AuthUserId { get; set; }
        public int RoleId { get; set; }
        public Role UserRole { get; set; }
        public AuthUser AuthUser { get; set; }
    }

}
