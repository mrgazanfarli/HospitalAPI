namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeAuthorName21 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Authors", "Name", c => c.String(nullable: false, maxLength: 21));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Authors", "Name", c => c.String(nullable: false, maxLength: 15));
        }
    }
}
