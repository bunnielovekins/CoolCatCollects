using CoolCatCollects.Ebay;
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
	}
}