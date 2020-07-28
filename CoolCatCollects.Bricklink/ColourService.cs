using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Bricklink.Models.Responses;
using CoolCatCollects.Data.Entities;
using CoolCatCollects.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Bricklink
{
	public class ColourService
	{
		/// <summary>
		/// Gets all Bricklink colours. If they aren't in the DB, get them from the API.
		/// </summary>
		/// <returns>Every colour in a model</returns>
		public IEnumerable<ColourModel> GetAll()
		{
			using (var repo = new BaseRepository<Colour>())
			{
				var colours = repo.FindAll();

				if (!colours.Any())
				{
					var apiService = new BricklinkApiService();
					var result = apiService.GetRequest<GetColoursResponse>("/colors");

					colours = result.data.Select(x => repo.Add(new Colour
					{
						ColourId = x.color_id,
						ColourCode = x.color_code,
						ColourType = x.color_type,
						Name = x.color_name
					}));
				}

				return colours.Select(x => new ColourModel(x));
			}
		}
	}
}
