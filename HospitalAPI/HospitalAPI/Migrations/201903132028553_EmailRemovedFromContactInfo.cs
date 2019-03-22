namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailRemovedFromContactInfo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ContactInfoes", "MailIcon");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContactInfoes", "MailIcon", c => c.String(maxLength: 100));
        }
    }
}
