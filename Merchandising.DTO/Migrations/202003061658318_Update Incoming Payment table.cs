namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIncomingPaymenttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IncomingLines", "GrossTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.IncomingLines", "Collections", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.IncomingLines", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Incomings", "InvoiceNo");
            DropColumn("dbo.Incomings", "InvoiceType");
            DropColumn("dbo.Incomings", "GrossTotal");
            DropColumn("dbo.Incomings", "Collections");
            DropColumn("dbo.Incomings", "Balance");
            DropColumn("dbo.Incomings", "DocTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Incomings", "DocTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Incomings", "Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Incomings", "Collections", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Incomings", "GrossTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Incomings", "InvoiceType", c => c.String());
            AddColumn("dbo.Incomings", "InvoiceNo", c => c.String());
            DropColumn("dbo.IncomingLines", "Balance");
            DropColumn("dbo.IncomingLines", "Collections");
            DropColumn("dbo.IncomingLines", "GrossTotal");
        }
    }
}
