namespace Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcomment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.Documents", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.Documents", "UpdatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "UpdatedBy");
            DropColumn("dbo.Documents", "UpdatedDate");
            DropColumn("dbo.Documents", "CreatedDate");
        }
    }
}
