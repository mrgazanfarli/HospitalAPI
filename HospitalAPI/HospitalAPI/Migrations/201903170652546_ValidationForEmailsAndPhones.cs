namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidationForEmailsAndPhones : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Emails", "EmailAddress", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Phones", "Number", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Phones", "Number", c => c.String());
            AlterColumn("dbo.Emails", "EmailAddress", c => c.String());
        }
    }
}
