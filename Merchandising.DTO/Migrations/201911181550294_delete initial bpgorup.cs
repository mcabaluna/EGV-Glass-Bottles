namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteinitialbpgorup : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.BpGroup");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BpGroup",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        BpType = c.Int(),
                        Name = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
        }
    }
}
