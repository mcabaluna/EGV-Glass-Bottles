namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCollectionandPayment : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Collection");
            DropTable("dbo.Payments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        DocEntry = c.Int(nullable: false, identity: true),
                        DocNum = c.Int(nullable: false),
                        BranchCode = c.String(),
                        CardCode = c.String(),
                        InvoiceNo = c.String(),
                        DueDate = c.DateTime(nullable: false),
                        GrossTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Collections = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DocTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ModeOfPayment = c.String(),
                        DatePaid = c.DateTime(nullable: false),
                        Remarks = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.DocEntry);
            
            CreateTable(
                "dbo.Collection",
                c => new
                    {
                        DocEntry = c.Int(nullable: false, identity: true),
                        DocNum = c.Int(nullable: false),
                        BranchCode = c.String(),
                        CardCode = c.String(),
                        InvoiceNo = c.String(),
                        DueDate = c.DateTime(nullable: false),
                        GrossTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Collections = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DocTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ModeOfPayment = c.String(),
                        DatePaid = c.DateTime(nullable: false),
                        Remarks = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.DocEntry);
            
        }
    }
}
