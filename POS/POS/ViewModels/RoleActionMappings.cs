using POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POS.ViewModels
{
    public class RoleActionMappings
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public List<ActionMaster> Action { get; set; }

        public List<RoleActionMapping> RoleActionMapping { get; set; }

    }
}