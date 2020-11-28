using CoolCatCollects.Ebay;
using CoolCatCollects.Ebay.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CoolCatCollects.Controllers
{
	public class eBaySlipController : Controller
	{
		private readonly eBayService _service;

		public eBaySlipController()
		{
			_service = new eBayService();
		}

		public ActionResult List(int page = 1, int perPage = 25)
		{
			var result = _service.GetOrders(perPage, page);

			return View(result);
		}

		public ActionResult PackingSlip(string orderId)
		{
			var order = _service.GetOrder(orderId);

			return View(order);
		}

		public ActionResult UnfulfilledOrders()
		{
			var orders = _service.GetUnfulfilledOrders();

			return View(orders);
		}

		public ActionResult CombinedSlip(string orders)
		{
			var model = new CombinedSlipModel();

			if (string.IsNullOrEmpty(orders))
			{
				model.Error = "No orders in url!";
				return View(model);
			}

			var ordersStr = orders.Split(',');

			if (ordersStr.Length == 1)
			{
				return RedirectToAction("PackingSlip", new { orderId = orders });
			}

			var orderModels = ordersStr.Select(x => _service.GetOrder(x));

			if (orderModels.Any(x => x.Buyer.Name != orderModels.First().Buyer.Name))
			{
				model.Error = "Buyer did not match!";
			}

			model.Orders = orderModels;

			return View(model);
		}

		public object GetItemDetails(string legacyItemId, string legacyVariationId)
		{
			var item = _service.GetItem(legacyItemId, legacyVariationId);

			return Json(new
			{
				imageUrl = item.Image,
				character = item.Character,
				sku = item.SKU
			}, "application/json", JsonRequestBehavior.AllowGet);
		}

		public struct CombinedSlipModel
		{
			public IEnumerable<EbayOrderModel> Orders { get; set; }
			public string Error { get; set; }

		}
	}
}