using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS.Models
{
    public class RoleActionMapping : BaseModel
    {
        public Guid RoleMasterId { get; set; }

        public Guid ActionMasterId { get; set; }
    }
}