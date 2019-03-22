namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoFixedInBlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "SmallPhoto", c => c.String(maxLength: 100));
            AddColumn("dbo.Blogs", "DetailsPhoto", c => c.String(maxLength: 100));
            DropColumn("dbo.Blogs", "Photo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Blogs", "Photo", c => c.String(maxLength: 100));
            DropColumn("dbo.Blogs", "DetailsPhoto");
            DropColumn("dbo.Blogs", "SmallPhoto");
        }
    }
}
