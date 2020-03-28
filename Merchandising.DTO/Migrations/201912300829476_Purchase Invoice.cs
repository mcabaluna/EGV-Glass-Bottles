namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseInvoice", "SInvoice", c => c.String());
            AddColumn("dbo.PurchaseInvoice", "TermId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseInvoice", "TermId");
            DropColumn("dbo.PurchaseInvoice", "SInvoice");
        }
    }
}
