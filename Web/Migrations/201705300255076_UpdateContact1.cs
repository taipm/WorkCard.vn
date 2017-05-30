namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateContact1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "MobilePhone", c => c.String());
            AddColumn("dbo.Contacts", "HomePhone", c => c.String());
            AddColumn("dbo.Contacts", "OfficePhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "OfficePhone");
            DropColumn("dbo.Contacts", "HomePhone");
            DropColumn("dbo.Contacts", "MobilePhone");
        }
    }
}
