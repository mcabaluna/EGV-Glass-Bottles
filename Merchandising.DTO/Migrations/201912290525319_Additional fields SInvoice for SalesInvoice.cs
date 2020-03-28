namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalfieldsSInvoiceforSalesInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesInvoice", "SInvoice", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesInvoice", "SInvoice");
        }
    }
}
