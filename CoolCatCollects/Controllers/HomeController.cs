using CoolCatCollects.Data.Entities;
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

			//var brepo = new BaseRepository<Part>();
			//var prepo = new PartInventoryRepository();

			//var part = brepo.FindOne(x => x.Number == "11062");

			//foreach(var inv in part.InventoryItems)
			//{
			//	prepo.CascadeDelete(inv);
			//}


			var repo = new InfoRepository();

			return View(new HomepageModel(repo.GetInfo()));
		}
	}
}
