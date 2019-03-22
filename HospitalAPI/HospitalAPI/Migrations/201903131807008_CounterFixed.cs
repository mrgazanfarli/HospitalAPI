namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CounterFixed : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Counters", "Icon");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Counters", "Icon", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
