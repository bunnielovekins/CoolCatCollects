using CoolCatCollects.Bricklink.Models.Responses;
using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Bricklink.Models
{
	public class SubsetPartsListModel
	{
		public SubsetPartsListModel()
		{
			Parts = new List<SubsetPartModel>();
		}

		public SubsetPartsListModel(GetSubsetResponse data)
		{
			Parts = data.data
				.SelectMany(x => x.entries)
				.Select(x => new SubsetPartModel(x));
		}

		public IEnumerable<SubsetPartModel> Parts { get; set; }
	}

	public class SubsetPartModel
	{
		public SubsetPartModel(BricklinkSubsetPartEntry entry)
		{
			var item = entry.item;

			if (entry.is_alternate)
			{
				Status = "Alt";
			}
			if (entry.is_counterpart)
			{
				Status = "CP";
			}

			Name = item.name;
			Number = item.no;
			Colour = new ColourModel();
			ColourId = entry.color_id;
			ColourName = "";
			Quantity = entry.quantity;
			ExtraQuantity = entry.extra_quantity;
			AveragePrice = "";
			Remark = "";
			MyPrice = "";
			Type = item.type;
			Category = item.category_id;
		}

		public SubsetPartModel()
		{

		}

		/// <summary>
		/// i.e. Alternate, Counterpart
		/// </summary>
		public string Status { get; set; }

		public string Name { get; set; }
		public string Number { get; set; }
		public ColourModel Colour { get; set; }
		public int ColourId { get; set; }
		public string ColourName { get; set; }
		public int Quantity { get; set; }
		public int ExtraQuantity { get; set; }
		public string AveragePrice { get; set; }
		public string Remark { get; set; }
		public string Type { get; set; }
		public int Category { get; set; }
		public string MyPrice { get; set; }
	}
}
