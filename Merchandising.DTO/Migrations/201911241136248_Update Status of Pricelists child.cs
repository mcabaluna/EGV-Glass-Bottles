namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStatusofPricelistschild : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PricelistLines", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PricelistUoM", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PricelistUoM", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.PricelistLines", "Status", c => c.Int(nullable: false));
        }
    }
}
