namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSequenceTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SequenceTable", "SeriesName", c => c.String());
            AddColumn("dbo.SequenceTable", "Status", c => c.Boolean(nullable: false));
            DropColumn("dbo.SequenceTable", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SequenceTable", "Active", c => c.Boolean(nullable: false));
            DropColumn("dbo.SequenceTable", "Status");
            DropColumn("dbo.SequenceTable", "SeriesName");
        }
    }
}
