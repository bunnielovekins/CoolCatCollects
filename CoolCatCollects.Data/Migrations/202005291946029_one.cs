namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class one : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PartInventories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InventoryId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        MyPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ColourId = c.Int(nullable: false),
                        ColourName = c.String(maxLength: 4000),
                        Location = c.String(maxLength: 4000),
                        Notes = c.String(maxLength: 4000),
                        LastUpdated = c.DateTime(nullable: false),
                        Part_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parts", t => t.Part_Id)
                .Index(t => t.Part_Id);
            
            CreateTable(
                "dbo.PartPriceInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AveragePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AveragePriceLocation = c.String(maxLength: 4000),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartInventories", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.Parts", "Number", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "Name", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "CategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.Parts", "ImageUrl", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "ThumbnailUrl", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "Weight", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "Description", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "LastUpdated", c => c.DateTime(nullable: false));
            DropColumn("dbo.Parts", "InventoryId");
            DropColumn("dbo.Parts", "ItemNo");
            DropColumn("dbo.Parts", "ItemName");
            DropColumn("dbo.Parts", "ColourId");
            DropColumn("dbo.Parts", "ColourName");
            DropColumn("dbo.Parts", "Quantity");
            DropColumn("dbo.Parts", "MyPrice");
            DropColumn("dbo.Parts", "Location");
            DropColumn("dbo.Parts", "AveragePrice");
            DropColumn("dbo.Parts", "AveragePriceLocation");
            DropColumn("dbo.Parts", "LastUpdatedInv");
            DropColumn("dbo.Parts", "LastUpdatedPrice");
            DropColumn("dbo.Parts", "LastUpdatedPart");
            DropColumn("dbo.Parts", "Notes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Parts", "Notes", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "LastUpdatedPart", c => c.DateTime(nullable: false));
            AddColumn("dbo.Parts", "LastUpdatedPrice", c => c.DateTime(nullable: false));
            AddColumn("dbo.Parts", "LastUpdatedInv", c => c.DateTime(nullable: false));
            AddColumn("dbo.Parts", "AveragePriceLocation", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "AveragePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Parts", "Location", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "MyPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Parts", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Parts", "ColourName", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "ColourId", c => c.Int(nullable: false));
            AddColumn("dbo.Parts", "ItemName", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "ItemNo", c => c.String(maxLength: 4000));
            AddColumn("dbo.Parts", "InventoryId", c => c.String(maxLength: 4000));
            DropForeignKey("dbo.PartPriceInfoes", "Id", "dbo.PartInventories");
            DropForeignKey("dbo.PartInventories", "Part_Id", "dbo.Parts");
            DropIndex("dbo.PartPriceInfoes", new[] { "Id" });
            DropIndex("dbo.PartInventories", new[] { "Part_Id" });
            DropColumn("dbo.Parts", "LastUpdated");
            DropColumn("dbo.Parts", "Description");
            DropColumn("dbo.Parts", "Weight");
            DropColumn("dbo.Parts", "ThumbnailUrl");
            DropColumn("dbo.Parts", "ImageUrl");
            DropColumn("dbo.Parts", "CategoryId");
            DropColumn("dbo.Parts", "Name");
            DropColumn("dbo.Parts", "Number");
            DropTable("dbo.PartPriceInfoes");
            DropTable("dbo.PartInventories");
        }
    }
}
