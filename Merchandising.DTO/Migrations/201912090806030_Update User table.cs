namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUsertable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "ContactNo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "ContactNo", c => c.Int(nullable: false));
        }
    }
}
