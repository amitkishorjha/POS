namespace POS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedatatype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillItems", "ProId", c => c.Guid(nullable: false));
            DropColumn("dbo.BillItems", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BillItems", "ProductId", c => c.Int(nullable: false));
            DropColumn("dbo.BillItems", "ProId");
        }
    }
}
