namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAboutToContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "About", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "About");
        }
    }
}
