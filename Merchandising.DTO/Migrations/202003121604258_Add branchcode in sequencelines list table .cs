namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addbranchcodeinsequencelineslisttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SequenceTableLines", "BranchCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SequenceTableLines", "BranchCode");
        }
    }
}
