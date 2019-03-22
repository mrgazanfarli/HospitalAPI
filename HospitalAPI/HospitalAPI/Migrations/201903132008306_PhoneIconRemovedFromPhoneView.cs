namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhoneIconRemovedFromPhoneView : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PhoneViews", "PhoneIcon");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PhoneViews", "PhoneIcon", c => c.String(maxLength: 100));
        }
    }
}
