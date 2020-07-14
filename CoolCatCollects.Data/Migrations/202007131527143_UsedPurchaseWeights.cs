namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsedPurchaseWeights : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsedPurchaseWeights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Colour = c.String(maxLength: 4000),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UsedPurchase_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UsedPurchases", t => t.UsedPurchase_Id)
                .Index(t => t.UsedPurchase_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsedPurchaseWeights", "UsedPurchase_Id", "dbo.UsedPurchases");
            DropIndex("dbo.UsedPurchaseWeights", new[] { "UsedPurchase_Id" });
            DropTable("dbo.UsedPurchaseWeights");
        }
    }
}
