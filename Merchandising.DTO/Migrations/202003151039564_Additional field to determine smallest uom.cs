namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Additionalfieldtodeterminesmallestuom : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemUoM", "isSmallestUoM", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemUoM", "isSmallestUoM");
        }
    }
}
