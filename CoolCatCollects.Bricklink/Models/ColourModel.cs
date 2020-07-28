using CoolCatCollects.Bricklink.Models.Responses;
using CoolCatCollects.Data.Entities;

namespace CoolCatCollects.Bricklink.Models
{
	public class ColourModel
	{
		public ColourModel()
		{

		}

		public ColourModel(BricklinkColour col)
		{
			Id = col.color_id;
			ColourCode = col.color_code;
			ColourType = col.color_type;
			Name = col.color_name;
		}

		public ColourModel(Colour col)
		{
			Id = col.ColourId;
			ColourCode = col.ColourCode;
			ColourType = col.ColourType;
			Name = col.Name;
		}

		public int Id { get; set; }
		/// <summary>
		/// Hex colour code, i.e. #ABCDEF
		/// </summary>
		public string ColourCode { get; set; }
		/// <summary>
		/// Trans, metallic, etc.
		/// </summary>
		public string ColourType { get; set; }
		public string Name { get; set; }
	}
}
