namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Department : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "Name", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.Departments", "Slug", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Departments", "Slug");
            DropColumn("dbo.Departments", "Name");
        }
    }
}
