namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrderToCounter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Counters", "Order", c => c.Byte(nullable: false));
            CreateIndex("dbo.Categories", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Categories", new[] { "Name" });
            DropColumn("dbo.Counters", "Order");
        }
    }
}
