namespace HospitalAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 15),
                        Photo = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Blogs", "AuthorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Blogs", "AuthorId");
            AddForeignKey("dbo.Blogs", "AuthorId", "dbo.Authors", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blogs", "AuthorId", "dbo.Authors");
            DropIndex("dbo.Blogs", new[] { "AuthorId" });
            DropColumn("dbo.Blogs", "AuthorId");
            DropTable("dbo.Authors");
        }
    }
}
