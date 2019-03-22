namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentIconFixed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DepartmentCards", "IconClass", c => c.String(nullable: false, maxLength: 30));
            AddColumn("dbo.DepartmentCards", "IconContent", c => c.String(nullable: false, maxLength: 15));
            DropColumn("dbo.DepartmentCards", "Icon");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DepartmentCards", "Icon", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.DepartmentCards", "IconContent");
            DropColumn("dbo.DepartmentCards", "IconClass");
        }
    }
}
