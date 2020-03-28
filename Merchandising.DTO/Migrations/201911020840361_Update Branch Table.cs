namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBranchTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Branch", "Active", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Branch", "Active", c => c.Int(nullable: false));
        }
    }
}
