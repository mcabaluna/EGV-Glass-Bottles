namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingUserRoleRolePageandRoleMenus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleMenus",
                c => new
                    {
                        RoleMenuId = c.String(nullable: false, maxLength: 128),
                        RoleName = c.String(nullable: false),
                        MenuName = c.String(nullable: false),
                        Visible = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.RoleMenuId);
            
            CreateTable(
                "dbo.RolePage",
                c => new
                    {
                        RolePageId = c.String(nullable: false, maxLength: 128),
                        RoleName = c.String(nullable: false),
                        Module = c.String(nullable: false),
                        Page = c.String(),
                        Button = c.String(),
                        Visible = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.RolePageId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        RoleName = c.String(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Roles");
            DropTable("dbo.RolePage");
            DropTable("dbo.RoleMenus");
        }
    }
}
