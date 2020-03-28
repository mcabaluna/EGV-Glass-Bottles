namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItemLedgerTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemLedger",
                c => new
                    {
                        LedgerId = c.Long(nullable: false, identity: true),
                        ItemCode = c.String(),
                        Branch = c.String(),
                        Warehouse = c.String(),
                        TransDate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ModuleName = c.String(),
                        DocumentId = c.String(),
                        BeginningBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BaseQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InventoryAction = c.String(),
                        EndingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.LedgerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ItemLedger");
        }
    }
}
