namespace InterviewManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEnum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Candidates", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Candidates", "Status", c => c.String());
        }
    }
}
