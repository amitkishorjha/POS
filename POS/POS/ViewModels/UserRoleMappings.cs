using POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS.ViewModels
{
    public class UserRoleMappings
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public List<RoleMaster> Role { get; set; }

        public List<UserRoleMapping> UserRoleMapping { get; set; }
    }
}