namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePricelistid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseInvoice", "PricelistId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseInvoice", "PricelistId");
        }
    }
}
