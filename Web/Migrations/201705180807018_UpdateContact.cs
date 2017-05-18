namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "HomeAddress", c => c.String());
            AddColumn("dbo.Contacts", "OfficeName", c => c.String());
            AddColumn("dbo.Contacts", "ProjectId", c => c.Guid());
            AddColumn("dbo.Contacts", "IssueId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "IssueId");
            DropColumn("dbo.Contacts", "ProjectId");
            DropColumn("dbo.Contacts", "OfficeName");
            DropColumn("dbo.Contacts", "HomeAddress");
        }
    }
}
