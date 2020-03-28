namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSequenceTableLines20200308 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SequenceTableLines", "SequenceTable_Id", "dbo.SequenceTable");
            DropIndex("dbo.SequenceTableLines", new[] { "SequenceTable_Id" });
            RenameColumn(table: "dbo.SequenceTableLines", name: "SequenceTable_Id", newName: "Id");
            AlterColumn("dbo.SequenceTableLines", "Id", c => c.Int(nullable: false));
            CreateIndex("dbo.SequenceTableLines", "Id");
            AddForeignKey("dbo.SequenceTableLines", "Id", "dbo.SequenceTable", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SequenceTableLines", "Id", "dbo.SequenceTable");
            DropIndex("dbo.SequenceTableLines", new[] { "Id" });
            AlterColumn("dbo.SequenceTableLines", "Id", c => c.Int());
            RenameColumn(table: "dbo.SequenceTableLines", name: "Id", newName: "SequenceTable_Id");
            CreateIndex("dbo.SequenceTableLines", "SequenceTable_Id");
            AddForeignKey("dbo.SequenceTableLines", "SequenceTable_Id", "dbo.SequenceTable", "Id");
        }
    }
}
