namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EbayOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "Image", c => c.String(maxLength: 4000));
            AddColumn("dbo.OrderItems", "CharacterName", c => c.String(maxLength: 4000));
            AddColumn("dbo.Orders", "BuyerUsername", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "BuyerUsername");
            DropColumn("dbo.OrderItems", "CharacterName");
            DropColumn("dbo.OrderItems", "Image");
        }
    }
}
