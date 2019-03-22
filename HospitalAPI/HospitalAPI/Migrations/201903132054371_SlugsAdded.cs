namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SlugsAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Slug", c => c.String(nullable: false));
            AddColumn("dbo.Doctors", "Slug", c => c.String(nullable: false));
            AlterColumn("dbo.Doctors", "Fullname", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Doctors", "Fullname", c => c.String(nullable: false, maxLength: 60));
            DropColumn("dbo.Doctors", "Slug");
            DropColumn("dbo.Blogs", "Slug");
        }
    }
}
