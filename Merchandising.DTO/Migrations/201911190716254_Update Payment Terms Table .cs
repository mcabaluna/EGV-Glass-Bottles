namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePaymentTermsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentTerms", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.PaymentTerms", "CreatedById", c => c.String());
            AddColumn("dbo.PaymentTerms", "ModifiedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.PaymentTerms", "ModifiedById", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentTerms", "ModifiedById");
            DropColumn("dbo.PaymentTerms", "ModifiedOn");
            DropColumn("dbo.PaymentTerms", "CreatedById");
            DropColumn("dbo.PaymentTerms", "CreatedOn");
        }
    }
}
