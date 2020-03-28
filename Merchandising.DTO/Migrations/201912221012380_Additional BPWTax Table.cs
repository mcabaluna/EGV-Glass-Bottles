namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalBPWTaxTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BpAddresses", newName: "BpAddress");
            CreateTable(
                "dbo.BpWTax",
                c => new
                    {
                        BpWTaxId = c.Int(nullable: false, identity: true),
                        CardCode = c.String(maxLength: 128),
                        WTCode = c.String(),
                    })
                .PrimaryKey(t => t.BpWTaxId)
                .ForeignKey("dbo.BusinessPartner", t => t.CardCode)
                .Index(t => t.CardCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BpWTax", "CardCode", "dbo.BusinessPartner");
            DropIndex("dbo.BpWTax", new[] { "CardCode" });
            DropTable("dbo.BpWTax");
            RenameTable(name: "dbo.BpAddress", newName: "BpAddresses");
        }
    }
}
