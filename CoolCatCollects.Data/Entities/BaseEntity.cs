using System.ComponentModel.DataAnnotations;

namespace CoolCatCollects.Data.Entities
{
	public class BaseEntity
	{
		[Key]
		public int Id { get; set; }
	}
}
