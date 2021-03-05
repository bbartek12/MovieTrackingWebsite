namespace MovieTrackingWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReview : DbMigration
    {
        public override void Up()
        {
        /*    AddColumn("dbo.Reviews", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Reviews", "User_Id");
            AddForeignKey("dbo.Reviews", "User_Id", "dbo.AspNetUsers", "Id");*/
        }
        
        public override void Down()
        {
          /*  DropForeignKey("dbo.Reviews", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Reviews", new[] { "User_Id" });
            DropColumn("dbo.Reviews", "User_Id");*/
        }
    }
}
