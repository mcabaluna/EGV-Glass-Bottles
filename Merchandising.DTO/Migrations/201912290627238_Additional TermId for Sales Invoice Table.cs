namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalTermIdforSalesInvoiceTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesInvoice", "TermId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesInvoice", "TermId");
        }
    }
}
