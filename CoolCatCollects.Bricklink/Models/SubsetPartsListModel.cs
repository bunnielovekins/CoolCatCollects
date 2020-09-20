using CoolCatCollects.Bricklink.Models.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
			Image = Type == "MINIFIG" ?
				$"https://img.bricklink.com/M/{item.no}.jpg" :
				$"https://img.bricklink.com/P/{entry.color_id}/{item.no}.jpg";

			FillRemarks();
		}

		public void FillRemarks()
		{
			if (!string.IsNullOrEmpty(Remark))
			{
				var regex = new Regex("(\\D*)(\\d*)");
				var match = regex.Match(Remark);
				if (match.Success)
				{
					if (match.Groups.Count > 0 && !string.IsNullOrEmpty(match.Groups[1].Value))
					{
						RemarkLetter1 = match.Groups[1].Value[0];
						RemarkLetter2 = match.Groups[1].Value.Length > 1 ? match.Groups[1].Value[1] : ' ';
						RemarkLetter3 = match.Groups[1].Value.Length > 2 ? match.Groups[1].Value[2] : ' ';
					}
					if (match.Groups.Count > 1)
					{
						if (int.TryParse(match.Groups[2].Value, out int tmpNum))
						{
							RemarkNumber = tmpNum;
						}
						else
						{
							RemarkNumber = 0;
						}
					}
				}
			}
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
		public string Image { get; set; }

		public char RemarkLetter1 { get; set; }
		public char RemarkLetter2 { get; set; }
		public char RemarkLetter3 { get; set; }

		public int RemarkNumber { get; set; }
	}
}
