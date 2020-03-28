namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRemarksfieldinsalesandpurchase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseInvoice", "Remarks", c => c.String());
            AddColumn("dbo.SalesInvoice", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesInvoice", "Remarks");
            DropColumn("dbo.PurchaseInvoice", "Remarks");
        }
    }
}
