namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recreatebpgroupmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BpGroup",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
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
        
        public override void Down()
        {
            DropTable("dbo.BpGroup");
        }
    }
}
