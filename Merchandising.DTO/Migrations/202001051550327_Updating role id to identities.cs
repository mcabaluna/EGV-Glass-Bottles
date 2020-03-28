namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatingroleidtoidentities : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Roles", "RoleId", c => c.Int(nullable: false, identity: true));
        }

        public override void Down()
        {
            DropColumn("dbo.Roles", "RoleId");
        }
    }
}
