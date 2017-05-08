namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIssue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkIssues", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.WorkIssues", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.WorkIssues", "UpdatedBy", c => c.String());
            AddColumn("dbo.WorkIssues", "Start", c => c.DateTime());
            AddColumn("dbo.WorkIssues", "End", c => c.DateTime());
            AddColumn("dbo.WorkIssues", "IsFinished", c => c.Boolean());
            AddColumn("dbo.WorkIssues", "IsDone", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkIssues", "IsDone");
            DropColumn("dbo.WorkIssues", "IsFinished");
            DropColumn("dbo.WorkIssues", "End");
            DropColumn("dbo.WorkIssues", "Start");
            DropColumn("dbo.WorkIssues", "UpdatedBy");
            DropColumn("dbo.WorkIssues", "UpdatedDate");
            DropColumn("dbo.WorkIssues", "CreatedDate");
        }
    }
}
