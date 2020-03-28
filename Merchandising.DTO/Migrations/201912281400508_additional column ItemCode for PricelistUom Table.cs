namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additionalcolumnItemCodeforPricelistUomTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PricelistUoM", "ItemId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PricelistUoM", "ItemId");
        }
    }
}
