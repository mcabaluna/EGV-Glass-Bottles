namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateItemOnHandPerWhseTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvAdjustmentLines", "Whse", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvAdjustmentLines", "Whse");
        }
    }
}
