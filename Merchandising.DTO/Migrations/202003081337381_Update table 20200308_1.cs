namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatetable20200308_1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SequenceTableLines", "SequenceTable_Id", c => c.Int());
            CreateIndex("dbo.SequenceTableLines", "SequenceTable_Id");
            AddForeignKey("dbo.SequenceTableLines", "SequenceTable_Id", "dbo.SequenceTable", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SequenceTableLines", "SequenceTable_Id", "dbo.SequenceTable");
            DropIndex("dbo.SequenceTableLines", new[] { "SequenceTable_Id" });
            DropColumn("dbo.SequenceTableLines", "SequenceTable_Id");
        }
    }
}
