namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrlModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Urls",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Domain = c.String(),
                        Address = c.String(),
                        Title = c.String(),
                        HtmlContent = c.String(),
                        IssueId = c.Guid(),
                        QuestionId = c.Guid(),
                        ProjectId = c.Guid(),
                        CommentId = c.Guid(),
                        StoryId = c.Guid(),
                        AnswerId = c.Guid(),
                        LastestCheck = c.DateTime(),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Urls");
        }
    }
}
