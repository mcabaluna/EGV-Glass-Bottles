namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddtotalPricefieldinItemUoM : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemUoM", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemUoM", "TotalPrice");
        }
    }
}
