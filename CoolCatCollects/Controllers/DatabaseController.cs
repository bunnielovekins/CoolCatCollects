using CoolCatCollects.Bricklink;
using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	public class DatabaseController : Controller
	{
		// GET: Database
		public ActionResult Index()
		{
			ViewBag.Title = "Database Page";

			var repo = new InfoRepository();

			return View(new DatabaseUpdateModel(repo.GetInfo()));
		}

		public ActionResult UpdateInventory(int colourId)
		{
			var service = new BricklinkService();

			var errors = service.UpdateInventoryForColour(colourId);

			return Json(new { success = errors.Any(), errors = errors.Any() ? errors.Aggregate((current, next) => current + ", " + next) : "" }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult UpdateInventoryDone()
		{
			var repo = new InfoRepository();

			var info = repo.GetInfo();

			info.InventoryLastUpdated = DateTime.Now;

			repo.Update(info);

			return Content("");
		}

		public ActionResult GetAllOrders()
		{
			var service = new BricklinkService();

			var orders = service.GetOrdersNotInDb();

			return Json(orders, JsonRequestBehavior.AllowGet);
		}

		public ActionResult AddOrder(string orderId)
		{
			var service = new BricklinkService();

			try
			{
				service.GetOrderWithItems(orderId);

				return Json(new { success = true, error = "" }, JsonRequestBehavior.AllowGet);
			}
			catch(Exception ex)
			{
				return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult UpdateOrdersDone()
		{
			var repo = new InfoRepository();

			var info = repo.GetInfo();

			info.OrdersLastUpdated = DateTime.Now;

			repo.Update(info);

			return Content("");
		}
	}
}