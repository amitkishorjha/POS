using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS.Models
{
    public class UserRoleMapping : BaseModel
    {
        public Guid RoleMasterId { get; set; }

        public Guid UserMasterId { get; set; }
    }
}