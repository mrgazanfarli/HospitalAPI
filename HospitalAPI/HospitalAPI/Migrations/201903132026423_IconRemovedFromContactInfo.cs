namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IconRemovedFromContactInfo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ContactInfoes", "PhoneIcon");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContactInfoes", "PhoneIcon", c => c.String(maxLength: 100));
        }
    }
}
