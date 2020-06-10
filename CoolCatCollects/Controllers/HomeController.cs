using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Models;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Title = "Cool Cat Collects";

			var repo = new InfoRepository();

			return View(new HomepageModel(repo.GetInfo()));
		}
	}
}
