namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Additionalfieldinitemtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemOnHandPerWhse", "ItemCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Items", "isSellItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "isPurchaseItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.Items", "isInvItem", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "isInvItem");
            DropColumn("dbo.Items", "isPurchaseItem");
            DropColumn("dbo.Items", "isSellItem");
            DropColumn("dbo.ItemOnHandPerWhse", "ItemCost");
        }
    }
}
