namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingSequenceTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SequenceTable", "InitialNum", c => c.String());
            AlterColumn("dbo.SequenceTable", "NextNumber", c => c.String());
            AlterColumn("dbo.SequenceTable", "LastNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SequenceTable", "LastNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.SequenceTable", "NextNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.SequenceTable", "InitialNum", c => c.Int(nullable: false));
        }
    }
}
