namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustWorkIssue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkIssues", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.WorkIssues", "IsFinished");
            DropColumn("dbo.WorkIssues", "IsDone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkIssues", "IsDone", c => c.Boolean());
            AddColumn("dbo.WorkIssues", "IsFinished", c => c.Boolean());
            DropColumn("dbo.WorkIssues", "Status");
        }
    }
}
