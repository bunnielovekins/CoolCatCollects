using CoolCatCollects.Bricklink;
using CoolCatCollects.Models;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	public class InventoryController : Controller
	{
		// GET: Inventory
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult GetByHistory(string location)
		{
			var service = new BricklinkService();

			var locations = service.GetHistoriesByLocation(location);

			var model = new LocationHistoryModel();
			model.AddRange(locations);
			model.Location = location;

			return View(model);
		}
	}
}