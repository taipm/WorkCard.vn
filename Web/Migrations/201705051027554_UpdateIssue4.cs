namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIssue4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkIssues", "Owner", c => c.String());
            AddColumn("dbo.WorkIssues", "IsQuaterly", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkIssues", "IsQuaterly");
            DropColumn("dbo.WorkIssues", "Owner");
        }
    }
}
