namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStatusdatatype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BpGroup", "BpType", c => c.Int());
            AlterColumn("dbo.BpGroup", "Status", c => c.Int());
            AlterColumn("dbo.ItemGroup", "Status", c => c.Int());
            AlterColumn("dbo.ModeOfPayment", "Active", c => c.Int());
            AlterColumn("dbo.PaymentTerms", "Status", c => c.Int());
            AlterColumn("dbo.UoM", "Status", c => c.Int());
            AlterColumn("dbo.Vat", "Type", c => c.Int());
            AlterColumn("dbo.Vat", "Status", c => c.Int());
            AlterColumn("dbo.Warehouse", "Status", c => c.Int());
            AlterColumn("dbo.WTax", "Type", c => c.Int());
            AlterColumn("dbo.WTax", "Status", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WTax", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.WTax", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.Warehouse", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.Vat", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.Vat", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.UoM", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.PaymentTerms", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.ModeOfPayment", "Active", c => c.Int(nullable: false));
            AlterColumn("dbo.ItemGroup", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.BpGroup", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.BpGroup", "BpType", c => c.Int(nullable: false));
        }
    }
}
