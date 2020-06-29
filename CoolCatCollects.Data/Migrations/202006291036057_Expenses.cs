namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Expenses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExpenseItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 4000),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Part_Id = c.Int(),
                        Expense_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PartInventories", t => t.Part_Id)
                .ForeignKey("dbo.Expenses", t => t.Expense_Id)
                .Index(t => t.Part_Id)
                .Index(t => t.Expense_Id);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        TaxCategory = c.String(maxLength: 4000),
                        Category = c.String(maxLength: 4000),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Source = c.String(maxLength: 4000),
                        Description = c.String(maxLength: 4000),
                        ExpenditureType = c.String(maxLength: 4000),
                        OrderNumber = c.String(maxLength: 4000),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Postage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Receipt = c.Boolean(nullable: false),
                        Notes = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewPurchases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        SetNumber = c.String(maxLength: 4000),
                        SetName = c.String(maxLength: 4000),
                        Theme = c.String(maxLength: 4000),
                        Promotions = c.String(maxLength: 4000),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Parts = c.Int(nullable: false),
                        TotalParts = c.Int(nullable: false),
                        PriceToPartOutRatio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Source = c.String(maxLength: 4000),
                        PaymentMethod = c.String(maxLength: 4000),
                        AveragePartOutValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MyPartOutValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpectedGrossProfit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpectedNetProfit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Uploaded = c.Boolean(nullable: false),
                        SellingNotes = c.String(maxLength: 4000),
                        Notes = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsedPurchases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Source = c.String(maxLength: 4000),
                        SourceUsername = c.String(maxLength: 4000),
                        OrderNumber = c.String(maxLength: 4000),
                        Price = c.String(maxLength: 4000),
                        PaymentMethod = c.String(maxLength: 4000),
                        Receipt = c.Boolean(nullable: false),
                        DistanceTravelled = c.String(maxLength: 4000),
                        Location = c.String(maxLength: 4000),
                        Postage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricePerKilo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CompleteSets = c.Boolean(nullable: false),
                        Notes = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExpenseItems", "Expense_Id", "dbo.Expenses");
            DropForeignKey("dbo.ExpenseItems", "Part_Id", "dbo.PartInventories");
            DropIndex("dbo.ExpenseItems", new[] { "Expense_Id" });
            DropIndex("dbo.ExpenseItems", new[] { "Part_Id" });
            DropTable("dbo.UsedPurchases");
            DropTable("dbo.NewPurchases");
            DropTable("dbo.Expenses");
            DropTable("dbo.ExpenseItems");
        }
    }
}
