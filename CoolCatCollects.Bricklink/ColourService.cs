using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Bricklink.Models.Responses;
using CoolCatCollects.Data.Entities;
using CoolCatCollects.Data.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolCatCollects.Bricklink
{
	public class ColourService
	{
		private BaseRepository<Colour> _repo;

		public ColourService()
		{
			_repo = new BaseRepository<Colour>();
		}

		public IEnumerable<ColourModel> GetAll()
		{
			var colours = _repo.FindAll();

			if (!colours.Any())
			{
				var apiService = new BricklinkApiService();
				var result = apiService.GetRequest("/colors");

				var responseModel = JsonConvert.DeserializeObject<GetColoursResponse>(result);

				if (responseModel.data == null)
				{
					result = apiService.GetRequest("/colors");

					responseModel = JsonConvert.DeserializeObject<GetColoursResponse>(result);

					if (responseModel.data == null)
					{
						throw new Exception("An error has occurred! Error code: " + responseModel.meta.code +
							", Error Message: " + responseModel.meta.message +
							", Error Description: " + responseModel.meta.description);
					}
				}

				colours = responseModel.data.Select(x => _repo.Add(new Colour
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
