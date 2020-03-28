namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserRole : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Roles");
            AddColumn("dbo.Roles", "Description", c => c.String());
            AlterColumn("dbo.Roles", "RoleId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Roles", "RoleId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Roles");
            AlterColumn("dbo.Roles", "RoleId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Roles", "Description");
            AddPrimaryKey("dbo.Roles", "RoleId");
        }
    }
}
