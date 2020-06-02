namespace CoolCatCollects.Data.Entities
{
	public class Colour : BaseEntity
	{
		public int ColourId { get; set; }
		public string ColourCode { get; set; }
		public string ColourType { get; set; }
		public string Name { get; set; }
	}
}
