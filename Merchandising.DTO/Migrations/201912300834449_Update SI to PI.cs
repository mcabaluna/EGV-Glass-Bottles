namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSItoPI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseInvoice", "PInvoice", c => c.String());
            DropColumn("dbo.PurchaseInvoice", "SInvoice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PurchaseInvoice", "SInvoice", c => c.String());
            DropColumn("dbo.PurchaseInvoice", "PInvoice");
        }
    }
}
