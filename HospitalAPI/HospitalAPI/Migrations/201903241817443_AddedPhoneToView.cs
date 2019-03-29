namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPhoneToView : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PhoneViews", "Number", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PhoneViews", "Number");
        }
    }
}
