namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhoneAndMailSeparated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Phones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.ContactInfoes", "Phone1");
            DropColumn("dbo.ContactInfoes", "Phone2");
            DropColumn("dbo.ContactInfoes", "Email1");
            DropColumn("dbo.ContactInfoes", "Email2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContactInfoes", "Email2", c => c.String(maxLength: 30));
            AddColumn("dbo.ContactInfoes", "Email1", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.ContactInfoes", "Phone2", c => c.String(maxLength: 30));
            AddColumn("dbo.ContactInfoes", "Phone1", c => c.String(nullable: false, maxLength: 30));
            DropTable("dbo.Phones");
            DropTable("dbo.Emails");
        }
    }
}
