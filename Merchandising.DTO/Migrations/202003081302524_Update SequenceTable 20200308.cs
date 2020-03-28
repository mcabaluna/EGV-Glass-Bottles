namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSequenceTable20200308 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SequenceTable", "ObjectCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SequenceTable", "ObjectCode");
        }
    }
}
