namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalpaidTodatefieldintransactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseInvoice", "PaidToDate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SalesInvoice", "PaidToDate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesInvoice", "PaidToDate");
            DropColumn("dbo.PurchaseInvoice", "PaidToDate");
        }
    }
}
