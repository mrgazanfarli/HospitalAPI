namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescAddedToDoctor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "Desc", c => c.String(nullable: false, maxLength: 130));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "Desc");
        }
    }
}
