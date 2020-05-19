using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace POS.Models
{
    public class POSContext : DbContext
    {
        public POSContext() : base("POSContext")
        {

        }

        public DbSet<UserMaster> UserMaster { get; set; }

        public DbSet<RoleMaster> RoleMaster { get; set; }

        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }

        public DbSet<ActionMaster> ActionMaster { get; set; }

        public DbSet<RoleActionMapping> RoleActionMapping { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<BillItems> BillItems { get; set; }

        public DbSet<Bill> Bill { get; set; }


    }
}