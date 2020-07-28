using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Core
{
	/// <summary>
	/// All the tax categories for expenses
	/// </summary>
	public static class TaxCategories
	{
		public class TaxCategory
		{
			public string Name { get; set; }
			public string Description { get; set; }
			public string Type { get; set; }

			public TaxCategory()
			{

			}

			public TaxCategory(string name, string type, string description)
			{
				Name = name;
				Description = description;
				Type = type;
			}

			public override string ToString()
			{
				return Name;
			}
		}

		public static TaxCategory OfficeCosts = new TaxCategory("Office Costs", "Allowable Expenses", "stationery or phone bills");
		public static TaxCategory TravelCosts = new TaxCategory("Travel Costs", "Allowable Expenses", "fuel, parking, train or bus fares");
		public static TaxCategory Clothing = new TaxCategory("Clothing", "Allowable Expenses", "uniforms");
		public static TaxCategory StaffCosts = new TaxCategory("Staff Costs", "Allowable Expenses", "salaries or subcontractor costs");
		public static TaxCategory Stock = new TaxCategory("Stock", "Allowable Expenses", "stock or raw materials");
		public static TaxCategory FinancialCosts = new TaxCategory("Financial Costs", "Allowable Expenses", "insurance or bank charges");
		public static TaxCategory BusinessPremises = new TaxCategory("Business Premises", "Allowable Expenses", "heating, lighting, business rates");
		public static TaxCategory Marketing = new TaxCategory("Marketing", "Allowable Expenses", "website costs");
		public static TaxCategory TrainingCourses = new TaxCategory("Training Courses", "Allowable Expenses", "refresher courses");

		public static TaxCategory Equipment = new TaxCategory("Equipment", "Capital Allowances", "");
		public static TaxCategory Machinery = new TaxCategory("Machinery", "Capital Allowances", "");
		public static TaxCategory Vehicles = new TaxCategory("Vehicles", "Capital Allowances", "cars, vans or lorries");

		public static Dictionary<string, TaxCategory> All = new List<TaxCategory>
			{
				OfficeCosts,
				TravelCosts,
				Clothing,
				StaffCosts,
				Stock,
				FinancialCosts,
				BusinessPremises,
				Marketing,
				TrainingCourses,
				Equipment,
				Machinery,
				Vehicles
			}.ToDictionary(x => x.ToString());
	}
}
