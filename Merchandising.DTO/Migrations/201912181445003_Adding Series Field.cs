namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingSeriesField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessPartner", "Series", c => c.String());
            AddColumn("dbo.Items", "Series", c => c.String());
            AddColumn("dbo.PurchaseInvoice", "Series", c => c.String());
            AddColumn("dbo.SalesInvoice", "Series", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesInvoice", "Series");
            DropColumn("dbo.PurchaseInvoice", "Series");
            DropColumn("dbo.Items", "Series");
            DropColumn("dbo.BusinessPartner", "Series");
        }
    }
}
