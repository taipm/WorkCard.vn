namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "IsRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "IsRequired");
        }
    }
}
