namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBusinessPartnerdetails : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BusinessPartner", "ContactNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BusinessPartner", "ContactNumber", c => c.Int(nullable: false));
        }
    }
}
