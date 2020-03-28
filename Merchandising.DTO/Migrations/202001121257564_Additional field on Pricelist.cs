namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalfieldonPricelist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pricelist", "WholesalePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Pricelist", "RetailPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pricelist", "RetailPrice");
            DropColumn("dbo.Pricelist", "WholesalePrice");
        }
    }
}
