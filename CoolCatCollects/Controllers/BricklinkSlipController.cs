using CoolCatCollects.Bricklink;
using System.Linq;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	public class BricklinkSlipController : Controller
	{
		private readonly BricklinkService _service;

		public BricklinkSlipController()
		{
			_service = new BricklinkService();
		}

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult List(string status = "PAID")
		{
			var result = _service.GetOrders(status);

			return View(result);
		}

		public ActionResult PartsList(string orderId)
		{
			var order = _service.GetOrderWithItems(orderId);

			return View(order);
		}

		public ActionResult PackingSlip(string orderId)
		{
			var order = _service.GetOrderWithItems(orderId);

			order.Items = order.Items
				.OrderBy(x => x.Colour)
				.ThenBy(x => x.Name);

			order.Items = order.Items.OrderBy(x => x.Colour).ThenBy(x => x.Name);

			return View(order);
		}
	}
}