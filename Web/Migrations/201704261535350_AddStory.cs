namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "StoryId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "StoryId");
        }
    }
}
