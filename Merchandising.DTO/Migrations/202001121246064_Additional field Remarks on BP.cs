namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalfieldRemarksonBP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessPartner", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BusinessPartner", "Remarks");
        }
    }
}
