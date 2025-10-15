namespace InterviewManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStatusDefaultJobPostion : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.JobPositions", "Status", c => c.String(nullable: false, defaultValue: "Open"));
        }

        public override void Down()
        {
            AlterColumn("dbo.JobPositions", "Status", c => c.String(nullable: false));
        }
    }
}
