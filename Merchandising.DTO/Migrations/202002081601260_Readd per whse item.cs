namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Readdperwhseitem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemOnHandPerWhse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemCode = c.String(maxLength: 128),
                        WhseId = c.String(),
                        OnHand = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Commited = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ordered = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemCode)
                .Index(t => t.ItemCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ItemOnHandPerWhse", "ItemCode", "dbo.Items");
            DropIndex("dbo.ItemOnHandPerWhse", new[] { "ItemCode" });
            DropTable("dbo.ItemOnHandPerWhse");
        }
    }
}
