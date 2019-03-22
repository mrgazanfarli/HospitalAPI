namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AboutAdvantages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AboutBodies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Photo = c.String(maxLength: 100),
                        Heading = c.String(nullable: false, maxLength: 30),
                        Title = c.String(nullable: false, maxLength: 70),
                        Text = c.String(nullable: false, storeType: "ntext"),
                        ButtonText = c.String(nullable: false, maxLength: 11),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AboutCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                        Desc = c.String(nullable: false, maxLength: 160),
                        Icon = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AboutIntroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 40),
                        Desc = c.String(nullable: false, storeType: "ntext"),
                        ButtonText = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Photo = c.String(maxLength: 100),
                        Date = c.DateTime(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Text = c.String(nullable: false, storeType: "ntext"),
                        SpecialText = c.String(storeType: "ntext"),
                        PostedBy = c.String(maxLength: 30),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CareCenters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        Desc = c.String(nullable: false, maxLength: 150),
                        Icon = c.String(maxLength: 100),
                        ButtonText = c.String(nullable: false, maxLength: 11),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContactDesc = c.String(nullable: false, maxLength: 80),
                        PhoneIcon = c.String(maxLength: 100),
                        MailIcon = c.String(maxLength: 100),
                        Phone1 = c.String(nullable: false, maxLength: 30),
                        Phone2 = c.String(maxLength: 30),
                        Email1 = c.String(nullable: false, maxLength: 30),
                        Email2 = c.String(maxLength: 30),
                        Facebook = c.String(maxLength: 100),
                        Twitter = c.String(maxLength: 100),
                        Google = c.String(maxLength: 100),
                        Linkedin = c.String(maxLength: 100),
                        VideoLink = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 60),
                        Lon = c.String(maxLength: 100),
                        Lat = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Counters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 30),
                        Value = c.Int(nullable: false),
                        Icon = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepartmentCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Icon = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 20),
                        Desc = c.String(nullable: false, maxLength: 110),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeadPosition = c.String(nullable: false, maxLength: 20),
                        HeadFullname = c.String(nullable: false, maxLength: 40),
                        EventDays = c.String(maxLength: 30),
                        EventName = c.String(maxLength: 35),
                        Address = c.String(nullable: false, maxLength: 35),
                        Email = c.String(nullable: false, maxLength: 40),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Photo = c.String(maxLength: 100),
                        Title = c.String(nullable: false, maxLength: 70),
                        Text = c.String(nullable: false, storeType: "ntext"),
                        Facebook = c.String(maxLength: 100),
                        Twitter = c.String(maxLength: 100),
                        Instagram = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DescCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                        Desc = c.String(nullable: false, maxLength: 125),
                        Photo = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(nullable: false, maxLength: 35),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 40),
                        Speciality = c.String(nullable: false, maxLength: 40),
                        Degree = c.String(nullable: false, maxLength: 40),
                        ExpertIn = c.String(nullable: false, maxLength: 50),
                        Category = c.String(nullable: false, maxLength: 30),
                        Facebook = c.String(maxLength: 100),
                        Twitter = c.String(maxLength: 100),
                        Instagram = c.String(maxLength: 100),
                        Linkedin = c.String(maxLength: 100),
                        Photo = c.String(maxLength: 100),
                        Fullname = c.String(nullable: false, maxLength: 60),
                        Text = c.String(nullable: false, storeType: "ntext"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Numbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorSliders = c.Int(nullable: false),
                        FeedbackSliders = c.Int(nullable: false),
                        BlogSliders = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OpeningHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 30),
                        Value = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhoneViews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Photo = c.String(nullable: false, maxLength: 100),
                        Title = c.String(nullable: false, maxLength: 100),
                        Text = c.String(nullable: false, maxLength: 125),
                        PhoneIcon = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderFullname = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Subject = c.String(maxLength: 100),
                        Number = c.String(maxLength: 30),
                        Message = c.String(nullable: false, storeType: "ntext"),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sliders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Desc = c.String(nullable: false, maxLength: 40),
                        BackImage = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blogs", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Blogs", new[] { "CategoryId" });
            DropTable("dbo.Sliders");
            DropTable("dbo.Questions");
            DropTable("dbo.PhoneViews");
            DropTable("dbo.OpeningHours");
            DropTable("dbo.Numbers");
            DropTable("dbo.Doctors");
            DropTable("dbo.DescCards");
            DropTable("dbo.Departments");
            DropTable("dbo.DepartmentCards");
            DropTable("dbo.Counters");
            DropTable("dbo.ContactInfoes");
            DropTable("dbo.CareCenters");
            DropTable("dbo.Categories");
            DropTable("dbo.Blogs");
            DropTable("dbo.AboutIntroes");
            DropTable("dbo.AboutCards");
            DropTable("dbo.AboutBodies");
            DropTable("dbo.AboutAdvantages");
        }
    }
}
