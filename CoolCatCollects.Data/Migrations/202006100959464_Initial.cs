namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Colours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ColourId = c.Int(nullable: false),
                        ColourCode = c.String(maxLength: 4000),
                        ColourType = c.String(maxLength: 4000),
                        Name = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PartInventoryLocationHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Location = c.String(maxLength: 4000),
                        PartInventory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartInventories", t => t.PartInventory_Id, cascadeDelete: true)
                .Index(t => t.PartInventory_Id);
            
            CreateTable(
                "dbo.PartInventories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InventoryId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        MyPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ColourId = c.Int(nullable: false),
                        ColourName = c.String(maxLength: 4000),
                        Location = c.String(maxLength: 4000),
                        Image = c.String(maxLength: 4000),
                        Description = c.String(maxLength: 4000),
                        Notes = c.String(maxLength: 4000),
                        LastUpdated = c.DateTime(nullable: false),
                        Part_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parts", t => t.Part_Id, cascadeDelete: true)
                .Index(t => t.Part_Id);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(maxLength: 4000),
                        ItemType = c.String(maxLength: 4000),
                        Name = c.String(maxLength: 4000),
                        CategoryId = c.Int(nullable: false),
                        ImageUrl = c.String(maxLength: 4000),
                        ThumbnailUrl = c.String(maxLength: 4000),
                        Weight = c.String(maxLength: 4000),
                        Description = c.String(maxLength: 4000),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PartPriceInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AveragePrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        AveragePriceLocation = c.String(maxLength: 4000),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartInventories", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartInventoryLocationHistories", "PartInventory_Id", "dbo.PartInventories");
            DropForeignKey("dbo.PartPriceInfoes", "Id", "dbo.PartInventories");
            DropForeignKey("dbo.PartInventories", "Part_Id", "dbo.Parts");
            DropIndex("dbo.PartPriceInfoes", new[] { "Id" });
            DropIndex("dbo.PartInventories", new[] { "Part_Id" });
            DropIndex("dbo.PartInventoryLocationHistories", new[] { "PartInventory_Id" });
            DropTable("dbo.PartPriceInfoes");
            DropTable("dbo.Parts");
            DropTable("dbo.PartInventories");
            DropTable("dbo.PartInventoryLocationHistories");
            DropTable("dbo.Colours");
        }
    }
}
