namespace MovieTrackingWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Reviews", new[] { "User_Id" });
            AddColumn("dbo.Reviews", "UserId", c => c.String());
            DropColumn("dbo.Reviews", "User_Id");
        }
        
        public override void Down()
        {
          /*  AddColumn("dbo.Reviews", "User_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Reviews", "UserId");
            CreateIndex("dbo.Reviews", "User_Id");
            AddForeignKey("dbo.Reviews", "User_Id", "dbo.AspNetUsers", "Id");*/
        }
    }
}
