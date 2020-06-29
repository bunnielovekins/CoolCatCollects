namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Title = c.String(maxLength: 4000),
                        Note = c.String(maxLength: 4000),
                        Category = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Logs");
        }
    }
}
