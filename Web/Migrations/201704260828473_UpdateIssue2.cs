namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIssue2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkIssues", "IsDaily", c => c.Boolean());
            AddColumn("dbo.WorkIssues", "IsWeekly", c => c.Boolean());
            AddColumn("dbo.WorkIssues", "IsMonthly", c => c.Boolean());
            AddColumn("dbo.WorkIssues", "IsYearly", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkIssues", "IsYearly");
            DropColumn("dbo.WorkIssues", "IsMonthly");
            DropColumn("dbo.WorkIssues", "IsWeekly");
            DropColumn("dbo.WorkIssues", "IsDaily");
        }
    }
}
