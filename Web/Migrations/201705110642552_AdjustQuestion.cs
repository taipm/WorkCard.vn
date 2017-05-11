namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "IssueId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "IssueId");
        }
    }
}
