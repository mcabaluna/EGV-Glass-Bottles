namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MajorupdateonStatusfieldfrominttobooldatatype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Branch", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BusinessPartner", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.BpGroup", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Collection", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.InvAdjustment", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ItemGroup", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Items", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ModeOfPayment", "Active", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Payments", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PaymentTerms", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UoM", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Pricelist", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SequenceTable", "Active", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Vat", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Warehouse", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.WTax", "Status", c => c.Boolean(nullable: false));
            DropColumn("dbo.Branch", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Branch", "Active", c => c.Int());
            AlterColumn("dbo.WTax", "Status", c => c.Int());
            AlterColumn("dbo.Warehouse", "Status", c => c.Int());
            AlterColumn("dbo.Vat", "Status", c => c.Int());
            AlterColumn("dbo.Users", "Status", c => c.Int());
            AlterColumn("dbo.SequenceTable", "Active", c => c.Int(nullable: false));
            AlterColumn("dbo.Pricelist", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.UoM", "Status", c => c.Int());
            AlterColumn("dbo.PaymentTerms", "Status", c => c.Int());
            AlterColumn("dbo.Payments", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.ModeOfPayment", "Active", c => c.Int());
            AlterColumn("dbo.Items", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.ItemGroup", "Status", c => c.Int());
            AlterColumn("dbo.InvAdjustment", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.Collection", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.BpGroup", "Status", c => c.Int());
            AlterColumn("dbo.BusinessPartner", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Branch", "Status");
        }
    }
}
