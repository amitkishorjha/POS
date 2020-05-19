namespace POS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstTime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionMasters",
                c => new
                    {
                        UniqueId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        FullActionName = c.String(nullable: false),
                        ControllerName = c.String(nullable: false),
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 30),
                        UpdatedBy = c.String(maxLength: 30),
                        DeletedBy = c.String(maxLength: 30),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        DeletedDate = c.DateTime(),
                        AccessCode = c.String(),
                    })
                .PrimaryKey(t => t.UniqueId);
            
            CreateTable(
                "dbo.RoleActionMappings",
                c => new
                    {
                        UniqueId = c.Guid(nullable: false),
                        RoleMasterId = c.Guid(nullable: false),
                        ActionMasterId = c.Guid(nullable: false),
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 30),
                        UpdatedBy = c.String(maxLength: 30),
                        DeletedBy = c.String(maxLength: 30),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        DeletedDate = c.DateTime(),
                        AccessCode = c.String(),
                    })
                .PrimaryKey(t => t.UniqueId);
            
            CreateTable(
                "dbo.RoleMasters",
                c => new
                    {
                        UniqueId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 30),
                        UpdatedBy = c.String(maxLength: 30),
                        DeletedBy = c.String(maxLength: 30),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        DeletedDate = c.DateTime(),
                        AccessCode = c.String(),
                    })
                .PrimaryKey(t => t.UniqueId);
            
            CreateTable(
                "dbo.UserMasters",
                c => new
                    {
                        UniqueId = c.Guid(nullable: false),
                        Username = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        LastLoginDate = c.String(),
                        Password = c.String(nullable: false, maxLength: 50),
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 30),
                        UpdatedBy = c.String(maxLength: 30),
                        DeletedBy = c.String(maxLength: 30),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        DeletedDate = c.DateTime(),
                        AccessCode = c.String(),
                    })
                .PrimaryKey(t => t.UniqueId);
            
            CreateTable(
                "dbo.UserRoleMappings",
                c => new
                    {
                        UniqueId = c.Guid(nullable: false),
                        RoleMasterId = c.Guid(nullable: false),
                        UserMasterId = c.Guid(nullable: false),
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 30),
                        UpdatedBy = c.String(maxLength: 30),
                        DeletedBy = c.String(maxLength: 30),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        DeletedDate = c.DateTime(),
                        AccessCode = c.String(),
                    })
                .PrimaryKey(t => t.UniqueId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserRoleMappings");
            DropTable("dbo.UserMasters");
            DropTable("dbo.RoleMasters");
            DropTable("dbo.RoleActionMappings");
            DropTable("dbo.ActionMasters");
        }
    }
}
