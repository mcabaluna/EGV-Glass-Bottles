namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubMenuFieldinRoleMenus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoleMenus", "SubMenuName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoleMenus", "SubMenuName");
        }
    }
}
