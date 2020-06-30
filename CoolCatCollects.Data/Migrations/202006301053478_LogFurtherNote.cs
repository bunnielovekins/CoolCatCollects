namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogFurtherNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "FurtherNote", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "FurtherNote");
        }
    }
}
