namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updateincomingpaymenttable20200307 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.IncomingLines", "GrossTotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.IncomingLines", "GrossTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
