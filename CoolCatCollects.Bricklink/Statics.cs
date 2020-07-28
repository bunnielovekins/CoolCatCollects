using CoolCatCollects.Bricklink.Models;
using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Bricklink
{
	/// <summary>
	/// Statics to be used in BL related things
	/// </summary>
	public static class Statics
	{
		public static string ConsumerKey = "0B69EB17FA3A488099B876498C6CBDBF";
		public static string ConsumerSecret = "54CFFA89431B4FCA8AC517A5B9F20E23";
		public static string TokenValue = "9AD379F021264DA7928736730EE1121B";
		public static string TokenSecret = "A2FE014F8C314CCEB2395B296C19EA97";
		public static string ApiUrl = "https://api.bricklink.com/api/store/v1/";

		/// <summary>
		/// All the colours in a dictionary sorted by Colour Id
		/// </summary>
		public static Dictionary<int, ColourModel> Colours
		{
			get
			{
				if (_colours != null && _colours.Any())
				{
					return _colours;
				}

				var service = new ColourService();

				var lst = service.GetAll().ToList();
				lst.Add(new ColourModel
				{
					Id = 0,
					ColourCode = "#FFFFFF",
					ColourType = "",
					Name = ""
				});

				_colours = lst.ToDictionary(x => x.Id);

				return _colours;
			}
		}

		private static Dictionary<int, ColourModel> _colours;
	}
}
