using CoolCatCollects.Data.Entities;
using System.Data.Entity;

namespace CoolCatCollects.Data
{
	public class EfContext : DbContext
	{
		public EfContext() : base("DataModelContext")
		{
			Database.SetInitializer(new CreateDatabaseIfNotExists<EfContext>());
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PartInventory>().Property(x => x.MyPrice).HasPrecision(18, 4);
			modelBuilder.Entity<PartPriceInfo>().Property(x => x.AveragePrice).HasPrecision(18, 4);
			modelBuilder.Entity<Order>().Property(x => x.Subtotal).HasPrecision(18, 4);
			modelBuilder.Entity<Order>().Property(x => x.Shipping).HasPrecision(18, 4);
			modelBuilder.Entity<Order>().Property(x => x.Deductions).HasPrecision(18, 4);
			modelBuilder.Entity<Order>().Property(x => x.ExtraCosts).HasPrecision(18, 4);
			modelBuilder.Entity<Order>().Property(x => x.GrandTotal).HasPrecision(18, 4);
			modelBuilder.Entity<OrderItem>().Property(x => x.UnitPrice).HasPrecision(18, 4);
		}

		public DbSet<Info> Infos { get; set; }
		public DbSet<Part> Parts { get; set; }
		public DbSet<PartInventory> PartInventorys { get; set; }
		public DbSet<PartPriceInfo> PartPriceInfos { get; set; }
		public DbSet<PartInventoryLocationHistory> PartInventoryLocationHistorys { get; set; }
		public DbSet<Colour> Colours { get; set; }
		public DbSet<EbayOrder> EbayOrders { get; set; }
		public DbSet<EbayOrderItem> EbayOrderItems { get; set; }
		public DbSet<BricklinkOrder> BricklinkOrders { get; set; }
		public DbSet<BricklinkOrderItem> BricklinkOrderItems { get; set; }
	}
}
