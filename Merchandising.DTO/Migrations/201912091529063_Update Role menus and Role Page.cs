namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRolemenusandRolePage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoleMenus", "RoleId", c => c.String(nullable: false));
            AddColumn("dbo.RolePage", "RoleId", c => c.String(nullable: false));
            DropColumn("dbo.RoleMenus", "RoleName");
            DropColumn("dbo.RolePage", "RoleName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RolePage", "RoleName", c => c.String(nullable: false));
            AddColumn("dbo.RoleMenus", "RoleName", c => c.String(nullable: false));
            DropColumn("dbo.RolePage", "RoleId");
            DropColumn("dbo.RoleMenus", "RoleId");
        }
    }
}
