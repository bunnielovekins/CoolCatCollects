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

			order.Items = order.Items.Select(x =>
			{
				var item = _service.GetItem(x.LegacyItemId, x.LegacyVariationId);

				if (item.localizedAspects != null && item.localizedAspects.Any())
				{
					foreach (var aspect in item.localizedAspects)
					{
						if (aspect.name == "Character")
						{
							x.Variant = aspect.value;
							break;
						}
					}
				}

				x.Image = item.image.imageUrl;

				return x;
			});

			return View(order);
		}

		public object GetItemDetails(string legacyItemId, string legacyVariationId)
		{
			var item = _service.GetItem(legacyItemId, legacyVariationId);

			var character = "";
			var sku = "";
			foreach (var aspect in item.localizedAspects)
			{
				if (aspect.name == "Character")
				{
					character = aspect.value;
				}
				else if (aspect.name == "SKU")
				{
					sku = aspect.value;
				}
			}

			return Json(new
			{
				item.image.imageUrl,
				character,
				sku
			}, "application/json", JsonRequestBehavior.AllowGet);
		}
	}
}