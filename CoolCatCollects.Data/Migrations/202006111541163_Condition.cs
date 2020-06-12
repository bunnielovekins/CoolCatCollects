namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Condition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartInventories", "Condition", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PartInventories", "Condition");
        }
    }
}
