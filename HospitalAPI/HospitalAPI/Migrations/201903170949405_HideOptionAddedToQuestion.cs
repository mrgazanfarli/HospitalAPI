namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HideOptionAddedToQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "IsHidden", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "IsHidden");
        }
    }
}
