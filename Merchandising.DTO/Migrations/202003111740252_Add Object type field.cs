namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddObjecttypefield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessPartner", "ObjectType", c => c.String());
            AddColumn("dbo.Incomings", "ObjectType", c => c.String());
            AddColumn("dbo.InvAdjustment", "ObjectType", c => c.String());
            AddColumn("dbo.Items", "ObjectType", c => c.String());
            AddColumn("dbo.Pricelist", "ObjectType", c => c.String());
            AddColumn("dbo.Pricelist", "Series", c => c.String());
            AddColumn("dbo.PurchaseInvoice", "ObjectType", c => c.String());
            AddColumn("dbo.SalesInvoice", "ObjectType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesInvoice", "ObjectType");
            DropColumn("dbo.PurchaseInvoice", "ObjectType");
            DropColumn("dbo.Pricelist", "Series");
            DropColumn("dbo.Pricelist", "ObjectType");
            DropColumn("dbo.Items", "ObjectType");
            DropColumn("dbo.InvAdjustment", "ObjectType");
            DropColumn("dbo.Incomings", "ObjectType");
            DropColumn("dbo.BusinessPartner", "ObjectType");
        }
    }
}
