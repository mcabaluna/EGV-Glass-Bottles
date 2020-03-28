namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBPTypedatatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BusinessPartner", "BpType", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BusinessPartner", "BpType", c => c.Int(nullable: false));
        }
    }
}
