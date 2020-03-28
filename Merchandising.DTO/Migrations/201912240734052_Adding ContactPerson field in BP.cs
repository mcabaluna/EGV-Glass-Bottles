namespace Merchandising.DTO.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingContactPersonfieldinBP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessPartner", "ContactPerson", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BusinessPartner", "ContactPerson");
        }
    }
}
