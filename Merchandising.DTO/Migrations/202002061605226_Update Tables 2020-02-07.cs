namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTables20200207 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvAdjustment", "InvoiceStatus", c => c.Int(nullable: false));
            AddColumn("dbo.InvAdjustment", "Series", c => c.String());
            AddColumn("dbo.InvAdjustment", "InvAdjustmentNo", c => c.String());
            AlterColumn("dbo.InvAdjustment", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvAdjustment", "Status", c => c.Boolean(nullable: false));
            DropColumn("dbo.InvAdjustment", "InvAdjustmentNo");
            DropColumn("dbo.InvAdjustment", "Series");
            DropColumn("dbo.InvAdjustment", "InvoiceStatus");
        }
    }
}
