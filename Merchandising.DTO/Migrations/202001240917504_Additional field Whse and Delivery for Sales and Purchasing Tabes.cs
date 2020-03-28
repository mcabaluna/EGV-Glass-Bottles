namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalfieldWhseandDeliveryforSalesandPurchasingTabes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseInvoiceLines", "Whse", c => c.String());
            AddColumn("dbo.PurchaseInvoice", "Deliverydate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SalesInvoiceLines", "Whse", c => c.String());
            AddColumn("dbo.SalesInvoice", "Deliverydate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesInvoice", "Deliverydate");
            DropColumn("dbo.SalesInvoiceLines", "Whse");
            DropColumn("dbo.PurchaseInvoice", "Deliverydate");
            DropColumn("dbo.PurchaseInvoiceLines", "Whse");
        }
    }
}
