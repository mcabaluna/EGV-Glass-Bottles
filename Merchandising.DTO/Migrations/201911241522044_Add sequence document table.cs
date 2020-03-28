namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addsequencedocumenttable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SequenceDocument",
                c => new
                    {
                        Document = c.String(nullable: false, maxLength: 128),
                        DocumentName = c.String(),
                        DocType = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Document);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SequenceDocument");
        }
    }
}
