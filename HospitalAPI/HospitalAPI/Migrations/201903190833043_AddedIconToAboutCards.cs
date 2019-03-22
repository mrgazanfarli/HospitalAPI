namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIconToAboutCards : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AboutCards", "IconClass", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.CareCenters", "IconClass", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CareCenters", "IconClass", c => c.String(maxLength: 100));
            DropColumn("dbo.AboutCards", "IconClass");
        }
    }
}
