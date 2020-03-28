namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalWholeSalesQtyfieldonItemTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "WholeSaleQty", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "WholeSaleQty");
        }
    }
}
