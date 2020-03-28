namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BusinessPartners", newName: "BusinessPartner");
            RenameTable(name: "dbo.BpGroups", newName: "BpGroup");
            RenameTable(name: "dbo.Branches", newName: "Branch");
            RenameTable(name: "dbo.Collections", newName: "Collection");
            RenameTable(name: "dbo.InvAdjustments", newName: "InvAdjustment");
            RenameTable(name: "dbo.ItemGroups", newName: "ItemGroup");
            RenameTable(name: "dbo.ItemOnHandPerWhses", newName: "ItemOnHandPerWhse");
            RenameTable(name: "dbo.ItemUoMs", newName: "ItemUoM");
            RenameTable(name: "dbo.ModeOfPayments", newName: "ModeOfPayment");
            RenameTable(name: "dbo.UoMs", newName: "UoM");
            RenameTable(name: "dbo.Pricelists", newName: "Pricelist");
            RenameTable(name: "dbo.PricelistUoMs", newName: "PricelistUoM");
            RenameTable(name: "dbo.PurchaseInvoices", newName: "PurchaseInvoice");
            RenameTable(name: "dbo.SalesInvoices", newName: "SalesInvoice");
            RenameTable(name: "dbo.SequenceTables", newName: "SequenceTable");
            RenameTable(name: "dbo.Vats", newName: "Vat");
            RenameTable(name: "dbo.Warehouses", newName: "Warehouse");
            RenameTable(name: "dbo.WTaxes", newName: "WTax");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.WTax", newName: "WTaxes");
            RenameTable(name: "dbo.Warehouse", newName: "Warehouses");
            RenameTable(name: "dbo.Vat", newName: "Vats");
            RenameTable(name: "dbo.SequenceTable", newName: "SequenceTables");
            RenameTable(name: "dbo.SalesInvoice", newName: "SalesInvoices");
            RenameTable(name: "dbo.PurchaseInvoice", newName: "PurchaseInvoices");
            RenameTable(name: "dbo.PricelistUoM", newName: "PricelistUoMs");
            RenameTable(name: "dbo.Pricelist", newName: "Pricelists");
            RenameTable(name: "dbo.UoM", newName: "UoMs");
            RenameTable(name: "dbo.ModeOfPayment", newName: "ModeOfPayments");
            RenameTable(name: "dbo.ItemUoM", newName: "ItemUoMs");
            RenameTable(name: "dbo.ItemOnHandPerWhse", newName: "ItemOnHandPerWhses");
            RenameTable(name: "dbo.ItemGroup", newName: "ItemGroups");
            RenameTable(name: "dbo.InvAdjustment", newName: "InvAdjustments");
            RenameTable(name: "dbo.Collection", newName: "Collections");
            RenameTable(name: "dbo.Branch", newName: "Branches");
            RenameTable(name: "dbo.BpGroup", newName: "BpGroups");
            RenameTable(name: "dbo.BusinessPartner", newName: "BusinessPartners");
        }
    }
}
