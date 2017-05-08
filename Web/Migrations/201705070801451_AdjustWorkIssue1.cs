namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustWorkIssue1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkIssues", "Message", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkIssues", "Message");
        }
    }
}
