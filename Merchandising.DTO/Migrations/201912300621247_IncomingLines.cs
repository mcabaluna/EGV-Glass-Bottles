namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncomingLines : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IncomingLines",
                c => new
                    {
                        LineID = c.Int(nullable: false, identity: true),
                        DocEntry = c.Int(nullable: false),
                        LineNum = c.Int(nullable: false),
                        DocNum = c.Int(nullable: false),
                        InvType = c.String(),
                        SumApplied = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DocTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceNo = c.String(),
                    })
                .PrimaryKey(t => t.LineID)
                .ForeignKey("dbo.Incomings", t => t.DocEntry, cascadeDelete: true)
                .Index(t => t.DocEntry);
            
            AddColumn("dbo.Incomings", "PaymentNo", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IncomingLines", "DocEntry", "dbo.Incomings");
            DropIndex("dbo.IncomingLines", new[] { "DocEntry" });
            DropColumn("dbo.Incomings", "PaymentNo");
            DropTable("dbo.IncomingLines");
        }
    }
}
