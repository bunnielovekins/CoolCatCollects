namespace CoolCatCollects.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpensesUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExpenseItems", "Part_Id", "dbo.PartInventories");
            DropForeignKey("dbo.ExpenseItems", "Expense_Id", "dbo.Expenses");
            DropIndex("dbo.ExpenseItems", new[] { "Part_Id" });
            DropIndex("dbo.ExpenseItems", new[] { "Expense_Id" });
            AddColumn("dbo.Expenses", "Item", c => c.String(maxLength: 4000));
            AddColumn("dbo.Expenses", "Quantity", c => c.String(maxLength: 4000));
            DropColumn("dbo.Expenses", "Amount");
            DropColumn("dbo.Expenses", "Description");
            DropTable("dbo.ExpenseItems");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Expenses", "Description", c => c.String(maxLength: 4000));
            AddColumn("dbo.Expenses", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Expenses", "Quantity");
            DropColumn("dbo.Expenses", "Item");
            CreateIndex("dbo.ExpenseItems", "Expense_Id");
            CreateIndex("dbo.ExpenseItems", "Part_Id");
            AddForeignKey("dbo.ExpenseItems", "Expense_Id", "dbo.Expenses", "Id");
            AddForeignKey("dbo.ExpenseItems", "Part_Id", "dbo.PartInventories", "Id");
        }
    }
}
