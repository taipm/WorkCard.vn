namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIssue3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkIssues", "TimeToDo", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkIssues", "TimeToDo");
        }
    }
}
