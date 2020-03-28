namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreationsofTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditTrailLogs",
                c => new
                    {
                        AuditId = c.Int(nullable: false, identity: true),
                        Document = c.String(),
                        Mode = c.String(),
                        Remarks = c.String(),
                        UpdatedBy = c.String(),
                        UpdatedTime = c.DateTime(nullable: false),
                        Branch = c.String(),
                        ComputerName = c.String(),
                        IpAddress = c.String(),
                        ReferenceNo = c.String(),
                    })
                .PrimaryKey(t => t.AuditId);
            
            CreateTable(
                "dbo.BpAddresses",
                c => new
                    {
                        BpAddressId = c.Int(nullable: false, identity: true),
                        CardCode = c.String(maxLength: 128),
                        Block = c.String(),
                        Street = c.String(),
                        CityId = c.Int(nullable: false),
                        ProvId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BpAddressId)
                .ForeignKey("dbo.BusinessPartners", t => t.CardCode)
                .Index(t => t.CardCode);
            
            CreateTable(
                "dbo.BusinessPartners",
                c => new
                    {
                        CardCode = c.String(nullable: false, maxLength: 128),
                        CardName = c.String(),
                        BpType = c.Int(nullable: false),
                        BpCode = c.String(),
                        Tin = c.String(),
                        VatCode = c.String(),
                        WithWTax = c.Boolean(nullable: false),
                        Address = c.String(),
                        ContactNumber = c.Int(nullable: false),
                        Email = c.String(),
                        PricelistId = c.String(),
                        TermId = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.CardCode);
            
            CreateTable(
                "dbo.BpGroups",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        BpType = c.Int(nullable: false),
                        Name = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Active = c.Int(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        ProvCode = c.String(),
                        ProvName = c.String(),
                        CityName = c.String(),
                        AreaCode = c.Int(nullable: false),
                        ZipCode = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CityId);
            
            CreateTable(
                "dbo.Collections",
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
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.DocEntry);
            
            CreateTable(
                "dbo.InvAdjustmentLines",
                c => new
                    {
                        LineId = c.Int(nullable: false, identity: true),
                        DocEntry = c.Int(nullable: false),
                        LineNum = c.Int(nullable: false),
                        ItemCode = c.String(),
                        ItemName = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UoM = c.String(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LineTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.LineId)
                .ForeignKey("dbo.InvAdjustments", t => t.DocEntry, cascadeDelete: true)
                .Index(t => t.DocEntry);
            
            CreateTable(
                "dbo.InvAdjustments",
                c => new
                    {
                        DocEntry = c.Int(nullable: false, identity: true),
                        DocNum = c.Int(nullable: false),
                        BranchCode = c.String(),
                        Type = c.Int(nullable: false),
                        Reason = c.String(),
                        Reference = c.String(),
                        Date = c.DateTime(nullable: false),
                        DocTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.DocEntry);
            
            CreateTable(
                "dbo.ItemGroups",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.ItemOnHandPerWhses",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ItemCode = c.String(maxLength: 128),
                        WhseId = c.String(),
                        OnHand = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Commited = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ordered = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemCode)
                .Index(t => t.ItemCode);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemCode = c.String(nullable: false, maxLength: 128),
                        ItemName = c.String(),
                        GroupCode = c.String(),
                        WtaxId = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.ItemCode);
            
            CreateTable(
                "dbo.ItemUoMs",
                c => new
                    {
                        UoMItemId = c.Int(nullable: false, identity: true),
                        LineNum = c.Int(nullable: false),
                        ItemCode = c.String(maxLength: 128),
                        UoM = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BaseUoM = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.UoMItemId)
                .ForeignKey("dbo.Items", t => t.ItemCode)
                .Index(t => t.ItemCode);
            
            CreateTable(
                "dbo.ModeOfPayments",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Active = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
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
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.DocEntry);
            
            CreateTable(
                "dbo.PaymentTerms",
                c => new
                    {
                        TermId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        NoOfDays = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TermId);
            
            CreateTable(
                "dbo.PricelistLines",
                c => new
                    {
                        PLineId = c.Int(nullable: false, identity: true),
                        LineNum = c.Int(nullable: false),
                        PricelistId = c.String(maxLength: 128),
                        ItemId = c.String(),
                        ItemName = c.String(),
                        UoMCode = c.String(maxLength: 128),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PLineId)
                .ForeignKey("dbo.UoMs", t => t.UoMCode)
                .ForeignKey("dbo.Pricelists", t => t.PricelistId)
                .Index(t => t.PricelistId)
                .Index(t => t.UoMCode);
            
            CreateTable(
                "dbo.UoMs",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.Pricelists",
                c => new
                    {
                        PricelistId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        BasePricelist = c.String(),
                        Factor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.PricelistId);
            
            CreateTable(
                "dbo.PricelistUoMs",
                c => new
                    {
                        PUomId = c.Int(nullable: false, identity: true),
                        LineNum = c.Int(nullable: false),
                        PricelistId = c.String(maxLength: 128),
                        PLineId = c.Int(nullable: false),
                        UoMCode = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Percentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PUomId)
                .ForeignKey("dbo.Pricelists", t => t.PricelistId)
                .Index(t => t.PricelistId);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        ProvCode = c.String(nullable: false, maxLength: 128),
                        ProvName = c.String(),
                    })
                .PrimaryKey(t => t.ProvCode);
            
            CreateTable(
                "dbo.PurchaseInvoiceLines",
                c => new
                    {
                        LineId = c.Int(nullable: false, identity: true),
                        LineNum = c.Int(nullable: false),
                        DocEntry = c.Int(nullable: false),
                        ItemCode = c.String(),
                        ItemName = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UoM = c.String(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceAfterDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LineTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Vat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GrossPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GrossTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.LineId)
                .ForeignKey("dbo.PurchaseInvoices", t => t.DocEntry, cascadeDelete: true)
                .Index(t => t.DocEntry);
            
            CreateTable(
                "dbo.PurchaseInvoices",
                c => new
                    {
                        DocEntry = c.Int(nullable: false, identity: true),
                        DocNum = c.Int(nullable: false),
                        BranchCode = c.String(),
                        CardCode = c.String(),
                        CardName = c.String(),
                        Reference = c.String(),
                        Status = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        DocTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VatAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GrossTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.DocEntry);
            
            CreateTable(
                "dbo.SalesInvoiceLines",
                c => new
                    {
                        LineId = c.Int(nullable: false, identity: true),
                        LineNum = c.Int(nullable: false),
                        DocEntry = c.Int(nullable: false),
                        ItemCode = c.String(),
                        ItemName = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UoM = c.String(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceAfterDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LineTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Vat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GrossPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GrossTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.LineId)
                .ForeignKey("dbo.SalesInvoices", t => t.DocEntry, cascadeDelete: true)
                .Index(t => t.DocEntry);
            
            CreateTable(
                "dbo.SalesInvoices",
                c => new
                    {
                        DocEntry = c.Int(nullable: false, identity: true),
                        DocNum = c.Int(nullable: false),
                        BranchCode = c.String(),
                        CardCode = c.String(),
                        CardName = c.String(),
                        Reference = c.String(),
                        Status = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        DocTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WTaxAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VatAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GrossTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.DocEntry);
            
            CreateTable(
                "dbo.SequenceTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Document = c.String(),
                        Length = c.Int(nullable: false),
                        Prefix = c.String(),
                        BranchCode = c.String(),
                        Active = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Role = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        ContactNo = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        LastAccess = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Vats",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Percentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        EffectiveFrom = c.DateTime(nullable: false),
                        EffectiveTo = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Status = c.Int(nullable: false),
                        BranchCode = c.String(maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.Branches", t => t.BranchCode)
                .Index(t => t.BranchCode);
            
            CreateTable(
                "dbo.WTaxes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Percentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        EffectiveFrom = c.DateTime(nullable: false),
                        EffectiveTo = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Warehouses", "BranchCode", "dbo.Branches");
            DropForeignKey("dbo.SalesInvoiceLines", "DocEntry", "dbo.SalesInvoices");
            DropForeignKey("dbo.PurchaseInvoiceLines", "DocEntry", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.PricelistUoMs", "PricelistId", "dbo.Pricelists");
            DropForeignKey("dbo.PricelistLines", "PricelistId", "dbo.Pricelists");
            DropForeignKey("dbo.PricelistLines", "UoMCode", "dbo.UoMs");
            DropForeignKey("dbo.ItemUoMs", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.ItemOnHandPerWhses", "ItemCode", "dbo.Items");
            DropForeignKey("dbo.InvAdjustmentLines", "DocEntry", "dbo.InvAdjustments");
            DropForeignKey("dbo.BpAddresses", "CardCode", "dbo.BusinessPartners");
            DropIndex("dbo.Warehouses", new[] { "BranchCode" });
            DropIndex("dbo.SalesInvoiceLines", new[] { "DocEntry" });
            DropIndex("dbo.PurchaseInvoiceLines", new[] { "DocEntry" });
            DropIndex("dbo.PricelistUoMs", new[] { "PricelistId" });
            DropIndex("dbo.PricelistLines", new[] { "UoMCode" });
            DropIndex("dbo.PricelistLines", new[] { "PricelistId" });
            DropIndex("dbo.ItemUoMs", new[] { "ItemCode" });
            DropIndex("dbo.ItemOnHandPerWhses", new[] { "ItemCode" });
            DropIndex("dbo.InvAdjustmentLines", new[] { "DocEntry" });
            DropIndex("dbo.BpAddresses", new[] { "CardCode" });
            DropTable("dbo.WTaxes");
            DropTable("dbo.Warehouses");
            DropTable("dbo.Vats");
            DropTable("dbo.Users");
            DropTable("dbo.SequenceTables");
            DropTable("dbo.SalesInvoices");
            DropTable("dbo.SalesInvoiceLines");
            DropTable("dbo.PurchaseInvoices");
            DropTable("dbo.PurchaseInvoiceLines");
            DropTable("dbo.Provinces");
            DropTable("dbo.PricelistUoMs");
            DropTable("dbo.Pricelists");
            DropTable("dbo.UoMs");
            DropTable("dbo.PricelistLines");
            DropTable("dbo.PaymentTerms");
            DropTable("dbo.Payments");
            DropTable("dbo.ModeOfPayments");
            DropTable("dbo.ItemUoMs");
            DropTable("dbo.Items");
            DropTable("dbo.ItemOnHandPerWhses");
            DropTable("dbo.ItemGroups");
            DropTable("dbo.InvAdjustments");
            DropTable("dbo.InvAdjustmentLines");
            DropTable("dbo.Collections");
            DropTable("dbo.Cities");
            DropTable("dbo.Branches");
            DropTable("dbo.BpGroups");
            DropTable("dbo.BusinessPartners");
            DropTable("dbo.BpAddresses");
            DropTable("dbo.AuditTrailLogs");
        }
    }
}
