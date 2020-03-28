namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Incomings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incomings", "CardName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Incomings", "CardName");
        }
    }
}
