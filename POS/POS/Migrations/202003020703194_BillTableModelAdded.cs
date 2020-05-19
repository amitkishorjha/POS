namespace POS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillTableModelAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "TableNo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "TableNo");
        }
    }
}
