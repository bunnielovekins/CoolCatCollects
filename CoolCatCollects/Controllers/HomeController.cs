using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Title = "Cool Cat Collects";

			return View();
		}
	}
}
