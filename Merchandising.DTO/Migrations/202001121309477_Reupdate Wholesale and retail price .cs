namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReupdateWholesaleandretailprice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PricelistLines", "WholesalePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PricelistLines", "RetailPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Pricelist", "WholesalePrice");
            DropColumn("dbo.Pricelist", "RetailPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pricelist", "RetailPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Pricelist", "WholesalePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.PricelistLines", "RetailPrice");
            DropColumn("dbo.PricelistLines", "WholesalePrice");
        }
    }
}
