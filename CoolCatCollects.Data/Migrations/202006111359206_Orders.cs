namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LineItemId = c.String(maxLength: 4000),
                        LegacyItemId = c.String(maxLength: 4000),
                        LegacyVariationId = c.String(maxLength: 4000),
                        SKU = c.String(maxLength: 4000),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Order_Id = c.Int(),
                        Part_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .ForeignKey("dbo.PartInventories", t => t.Part_Id)
                .Index(t => t.Order_Id)
                .Index(t => t.Part_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.String(maxLength: 4000),
                        OrderDate = c.DateTime(nullable: false),
                        BuyerName = c.String(maxLength: 4000),
                        BuyerEmail = c.String(maxLength: 4000),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Shipping = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Deductions = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ExtraCosts = c.Decimal(nullable: false, precision: 18, scale: 4),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Status = c.Int(nullable: false),
                        TotalCount = c.Int(),
                        UniqueCount = c.Int(),
                        Weight = c.String(maxLength: 4000),
                        DriveThruSent = c.Boolean(),
                        ShippingMethod = c.String(maxLength: 4000),
                        LegacyOrderId = c.String(maxLength: 4000),
                        SalesRecordReference = c.String(maxLength: 4000),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Infoes", "OrdersLastUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "Part_Id", "dbo.PartInventories");
            DropForeignKey("dbo.OrderItems", "Order_Id", "dbo.Orders");
            DropIndex("dbo.OrderItems", new[] { "Part_Id" });
            DropIndex("dbo.OrderItems", new[] { "Order_Id" });
            DropColumn("dbo.Infoes", "OrdersLastUpdated");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
        }
    }
}
