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
		public string ColourCode { get; set; }
		public string ColourType { get; set; }
		public string Name { get; set; }
	}
}
