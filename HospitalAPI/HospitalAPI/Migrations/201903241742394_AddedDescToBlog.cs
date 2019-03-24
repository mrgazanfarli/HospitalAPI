namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDescToBlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Desc", c => c.String(nullable: false, maxLength: 110));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "Desc");
        }
    }
}
