namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIdofItemOnHandperwhsetable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ItemOnHandPerWhse");
            AlterColumn("dbo.ItemOnHandPerWhse", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ItemOnHandPerWhse", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ItemOnHandPerWhse");
            AlterColumn("dbo.ItemOnHandPerWhse", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ItemOnHandPerWhse", "Id");
        }
    }
}
