namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotosAndIconsFixed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareCenters", "IconClass", c => c.String(maxLength: 100));
            AddColumn("dbo.Counters", "IconClass", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.AboutBodies", "Photo", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Authors", "Photo", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Blogs", "SmallPhoto", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Blogs", "DetailsPhoto", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Departments", "Photo", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.DescCards", "Photo", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Doctors", "Photo", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Feedbacks", "ProfilePhoto", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.PhoneViews", "Photo", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Sliders", "BackImage", c => c.String(storeType: "ntext"));
            DropColumn("dbo.AboutCards", "IconClass");
            DropColumn("dbo.AboutCards", "IconContent");
            DropColumn("dbo.CareCenters", "Icon");
            DropColumn("dbo.DepartmentCards", "IconContent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DepartmentCards", "IconContent", c => c.String(nullable: false, maxLength: 15));
            AddColumn("dbo.CareCenters", "Icon", c => c.String(maxLength: 100));
            AddColumn("dbo.AboutCards", "IconContent", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.AboutCards", "IconClass", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Sliders", "BackImage", c => c.String(maxLength: 100));
            AlterColumn("dbo.PhoneViews", "Photo", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Feedbacks", "ProfilePhoto", c => c.String(maxLength: 100));
            AlterColumn("dbo.Doctors", "Photo", c => c.String(maxLength: 100));
            AlterColumn("dbo.DescCards", "Photo", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Departments", "Photo", c => c.String(maxLength: 100));
            AlterColumn("dbo.Blogs", "DetailsPhoto", c => c.String(maxLength: 100));
            AlterColumn("dbo.Blogs", "SmallPhoto", c => c.String(maxLength: 100));
            AlterColumn("dbo.Authors", "Photo", c => c.String(maxLength: 100));
            AlterColumn("dbo.AboutBodies", "Photo", c => c.String(maxLength: 100));
            DropColumn("dbo.Counters", "IconClass");
            DropColumn("dbo.CareCenters", "IconClass");
        }
    }
}
