namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIssue1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkIssues", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.WorkIssues", "UpdatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkIssues", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.WorkIssues", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
