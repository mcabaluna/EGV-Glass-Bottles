namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProvincedatatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BpAddress", "ProvId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BpAddress", "ProvId", c => c.Int(nullable: false));
        }
    }
}
