using CoolCatCollects.Core;
using CoolCatCollects.Models;
using System.Linq;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	public class PostageController : Controller
	{
		// GET: Postage
		public ActionResult Index(string price = "", string weight = "", string size = "")
		{
			if (string.IsNullOrEmpty(price) || string.IsNullOrEmpty(weight) || string.IsNullOrEmpty(size))
			{
				return View();
			}

			var postages = PostageCalculator.GetPostages(weight, price, size).OrderBy(x => x.Price);

			var model = new PostageModel
			{
				Economy = postages.FirstOrDefault(x => x.Type == PostageCalculator.ServiceType.Economy),
				Standard = postages.FirstOrDefault(x => x.Type == PostageCalculator.ServiceType.Standard),
				Express = postages.FirstOrDefault(x => x.Type == PostageCalculator.ServiceType.Express),
				Price = price,
				Weight = weight,
				Size = size == "LL" ? "Large Letter" :
						size == "SP" ? "Small Parcel" :
						"Parcel"
			};

			model.Alternatives = postages.Where(x => x.Name != model.Economy.Name && x.Name != model.Standard.Name && x.Name != model.Express.Name);

			return View(model);
		}
	}
}