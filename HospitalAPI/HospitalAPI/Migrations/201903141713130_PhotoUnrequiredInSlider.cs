namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoUnrequiredInSlider : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sliders", "BackImage", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sliders", "BackImage", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
