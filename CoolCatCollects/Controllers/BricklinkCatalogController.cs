using CoolCatCollects.Bricklink;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	public class BricklinkCatalogController : Controller
	{
		private readonly BricklinkService _service;

		public BricklinkCatalogController()
		{
			_service = new BricklinkService();
		}

		// GET: BricklinkCatalog
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult PartListFromSet(string set)
		{
			if (string.IsNullOrEmpty(set))
			{
				return RedirectToAction("Index");
			}

			if (!set.Contains("-"))
			{
				set += "-1";
			}

			var parts = _service.GetPartsFromSet(set);

			return View(model: parts);
		}
	}
}