namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePricelistUomTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PricelistUoM", "PLineId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PricelistUoM", "PLineId", c => c.Int(nullable: false));
        }
    }
}
