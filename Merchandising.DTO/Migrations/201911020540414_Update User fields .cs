namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserfields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Status", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Status", c => c.Int(nullable: false));
        }
    }
}
