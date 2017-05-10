namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRepeatType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkIssues", "Repeat", c => c.Int(nullable: false));
            DropColumn("dbo.WorkIssues", "IsDaily");
            DropColumn("dbo.WorkIssues", "IsWeekly");
            DropColumn("dbo.WorkIssues", "IsMonthly");
            DropColumn("dbo.WorkIssues", "IsQuaterly");
            DropColumn("dbo.WorkIssues", "IsYearly");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkIssues", "IsYearly", c => c.Boolean());
            AddColumn("dbo.WorkIssues", "IsQuaterly", c => c.Boolean());
            AddColumn("dbo.WorkIssues", "IsMonthly", c => c.Boolean());
            AddColumn("dbo.WorkIssues", "IsWeekly", c => c.Boolean());
            AddColumn("dbo.WorkIssues", "IsDaily", c => c.Boolean());
            DropColumn("dbo.WorkIssues", "Repeat");
        }
    }
}
