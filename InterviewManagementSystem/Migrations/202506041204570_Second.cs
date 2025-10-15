namespace InterviewManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Candidates", "CVPath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Candidates", "CVPath", c => c.String(nullable: false));
        }
    }
}
