namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSequenceTableLinesTable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.SequenceDocument");
            CreateTable(
                "dbo.SequenceTableLines",
                c => new
                    {
                        Series = c.Int(nullable: false, identity: true),
                        ObjectCode = c.Int(nullable: false),
                        SeriesName = c.String(maxLength: 8),
                        InitialNum = c.Int(nullable: false),
                        NextNumber = c.Int(nullable: false),
                        LastNum = c.Int(nullable: false),
                        BeginStr = c.String(maxLength: 20),
                        LastStr = c.String(maxLength: 20),
                        Remarks = c.String(maxLength: 50),
                        Locked = c.Boolean(nullable: false),
                        Indicator = c.Boolean(nullable: false),
                        NumSize = c.Int(nullable: false),
                        DocSubType = c.String(maxLength: 2),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedById = c.String(),
                        ModifiedOn = c.DateTime(nullable: false),
                        ModifiedById = c.String(),
                    })
                .PrimaryKey(t => t.Series);
            
            AddColumn("dbo.SequenceDocument", "ObjectCode", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.SequenceDocument", "DocSubType", c => c.String());
            AddColumn("dbo.SequenceTable", "DefaultSeries", c => c.Int(nullable: false));
            AddColumn("dbo.SequenceTable", "DocSubType", c => c.String());
            AddPrimaryKey("dbo.SequenceDocument", "ObjectCode");
            DropColumn("dbo.SequenceDocument", "Document");
            DropColumn("dbo.SequenceTable", "Document");
            DropColumn("dbo.SequenceTable", "DocType");
            DropColumn("dbo.SequenceTable", "SeriesName");
            DropColumn("dbo.SequenceTable", "Prefix");
            DropColumn("dbo.SequenceTable", "Suffix");
            DropColumn("dbo.SequenceTable", "InitialNum");
            DropColumn("dbo.SequenceTable", "NextNumber");
            DropColumn("dbo.SequenceTable", "LastNumber");
            DropColumn("dbo.SequenceTable", "BranchCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SequenceTable", "BranchCode", c => c.String());
            AddColumn("dbo.SequenceTable", "LastNumber", c => c.String());
            AddColumn("dbo.SequenceTable", "NextNumber", c => c.String());
            AddColumn("dbo.SequenceTable", "InitialNum", c => c.String());
            AddColumn("dbo.SequenceTable", "Suffix", c => c.String());
            AddColumn("dbo.SequenceTable", "Prefix", c => c.String());
            AddColumn("dbo.SequenceTable", "SeriesName", c => c.String());
            AddColumn("dbo.SequenceTable", "DocType", c => c.String());
            AddColumn("dbo.SequenceTable", "Document", c => c.String());
            AddColumn("dbo.SequenceDocument", "Document", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.SequenceDocument");
            DropColumn("dbo.SequenceTable", "DocSubType");
            DropColumn("dbo.SequenceTable", "DefaultSeries");
            DropColumn("dbo.SequenceDocument", "DocSubType");
            DropColumn("dbo.SequenceDocument", "ObjectCode");
            DropTable("dbo.SequenceTableLines");
            AddPrimaryKey("dbo.SequenceDocument", "Document");
        }
    }
}
