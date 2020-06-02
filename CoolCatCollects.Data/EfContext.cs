using CoolCatCollects.Data.Entities;
using System.Data.Entity;

namespace CoolCatCollects.Data
{
	public class EfContext : DbContext
	{
		public EfContext() : base("coolcat.sdf")
		{
			Database.SetInitializer(new CreateDatabaseIfNotExists<EfContext>());
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PartInventory>().Property(x => x.MyPrice).HasPrecision(18, 4);
			modelBuilder.Entity<PartPriceInfo>().Property(x => x.AveragePrice).HasPrecision(18, 4);
		}

		public DbSet<Part> Parts { get; set; }
		public DbSet<PartInventory> PartInventorys { get; set; }
		public DbSet<PartPriceInfo> PartPriceInfos { get; set; }
		public DbSet<Colour> Colours { get; set; }
	}
}
