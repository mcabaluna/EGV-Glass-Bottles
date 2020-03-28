namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSequenceTablewithnumbering : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SequenceTable", "DocType", c => c.String());
            AddColumn("dbo.SequenceTable", "Suffix", c => c.String());
            AddColumn("dbo.SequenceTable", "InitialNum", c => c.Int(nullable: false));
            AddColumn("dbo.SequenceTable", "NextNumber", c => c.Int(nullable: false));
            AddColumn("dbo.SequenceTable", "LastNumber", c => c.Int(nullable: false));
            DropColumn("dbo.SequenceTable", "Length");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SequenceTable", "Length", c => c.Int(nullable: false));
            DropColumn("dbo.SequenceTable", "LastNumber");
            DropColumn("dbo.SequenceTable", "NextNumber");
            DropColumn("dbo.SequenceTable", "InitialNum");
            DropColumn("dbo.SequenceTable", "Suffix");
            DropColumn("dbo.SequenceTable", "DocType");
        }
    }
}
