namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedButtonhrefToAbout : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AboutBodies", "ButtonURL", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AboutBodies", "ButtonURL");
        }
    }
}
