namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InventoryId = c.String(maxLength: 4000),
                        ItemNo = c.String(maxLength: 4000),
                        ItemType = c.String(maxLength: 4000),
                        ItemName = c.String(maxLength: 4000),
                        ColourId = c.Int(nullable: false),
                        ColourName = c.String(maxLength: 4000),
                        Quantity = c.Int(nullable: false),
                        MyPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Location = c.String(maxLength: 4000),
                        AveragePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AveragePriceLocation = c.String(maxLength: 4000),
                        LastUpdatedInv = c.DateTime(nullable: false),
                        LastUpdatedPrice = c.DateTime(nullable: false),
                        LastUpdatedPart = c.DateTime(nullable: false),
                        Notes = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Parts");
        }
    }
}
