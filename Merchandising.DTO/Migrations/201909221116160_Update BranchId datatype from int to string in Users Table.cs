namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBranchIddatatypefrominttostringinUsersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "BranchCode", c => c.String());
            DropColumn("dbo.Users", "BranchId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "BranchId", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "BranchCode");
        }
    }
}
