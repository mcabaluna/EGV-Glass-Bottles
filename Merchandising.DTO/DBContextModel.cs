using System.Data.Entity;
using Merchandising.DTO.Models;

namespace Merchandising.DTO
{
    public class DbContextModel : DbContext
    {
        public DbContextModel() : base("LocalConnection")
        {

        }

        public DbSet<AuditTrailLogs> AuditTrailLogs { get; set; }
        public DbSet<BpGroup> BpGroups { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BusinessPartner> BusinessPartners { get; set; }
        public DbSet<BpAddress> BpAddresses { get; set; }
        public DbSet<BpWTax> BpWTax { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Provinces> Provinces { get; set; }
        public DbSet<InvAdjustment> InvAdjustments { get; set; }
        public DbSet<InvAdjustmentLines> InvAdjustmentLines { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<ItemLedger> ItemLedger { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<ItemUoM> ItemUoM { get; set; }
        public DbSet<ItemOnHandPerWhse> ItemOnHandPerWhse { get; set; }
        public DbSet<ModeOfPayment> ModeOfPayments { get; set; }
        public DbSet<Incomings> Incomings { get; set; }
        public DbSet<PaymentTerms> PaymentTerms { get; set; }
        public DbSet<Pricelist> Pricelists { get; set; }
        public DbSet<PricelistLines> PricelistLines { get; set; }
        public DbSet<PricelistUoM> PricelistUoM { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<PurchaseInvoiceLines> PurchaseInvoiceLines { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceLines> SalesInvoiceLines { get; set; }
        public DbSet<SequenceTable> SequenceTables { get; set; }
        public DbSet<SequenceTableLines> SequenceTableLines { get; set; }
        public DbSet<SequenceDocument> SequenceDocument { get; set; }
        public DbSet<UoM> UoM { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<RolePage> RolePage { get; set; }
        public DbSet<RoleMenus> RoleMenus { get; set; }
        public DbSet<Vat> Vat { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WTax> WTaxs { get; set; }

    }
}