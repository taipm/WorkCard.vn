namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSomeObjects : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "ProjectId", c => c.Guid());
            AddColumn("dbo.Comments", "IssueId", c => c.Guid());
            AddColumn("dbo.Comments", "QuestionId", c => c.Guid());
            AddColumn("dbo.WorkIssues", "ProjectId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkIssues", "ProjectId");
            DropColumn("dbo.Comments", "QuestionId");
            DropColumn("dbo.Comments", "IssueId");
            DropColumn("dbo.Comments", "ProjectId");
        }
    }
}
