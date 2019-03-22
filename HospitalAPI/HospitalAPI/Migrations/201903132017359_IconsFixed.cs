namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IconsFixed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AboutCards", "IconClass", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.AboutCards", "IconContent", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.AboutCards", "Icon");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AboutCards", "Icon", c => c.String(maxLength: 100));
            DropColumn("dbo.AboutCards", "IconContent");
            DropColumn("dbo.AboutCards", "IconClass");
        }
    }
}
