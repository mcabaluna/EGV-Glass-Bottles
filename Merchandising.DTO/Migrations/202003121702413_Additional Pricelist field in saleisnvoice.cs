namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalPricelistfieldinsaleisnvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesInvoice", "PricelistId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesInvoice", "PricelistId");
        }
    }
}
