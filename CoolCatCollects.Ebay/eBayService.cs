using CoolCatCollects.Ebay.Models;
using CoolCatCollects.Ebay.Models.Responses;
using Newtonsoft.Json;
using System;

namespace CoolCatCollects.Ebay
{
	public class eBayService
	{
		private readonly eBayApiService _service;

		public eBayService()
		{
			_service = new eBayApiService();
		}

		public EbayOrderModel GetOrder(string orderNumber)
		{
			var response = _service.GetRequest($"sell/fulfillment/v1/order/{orderNumber}");

			var obj = JsonConvert.DeserializeObject<GetOrderResponseModel>(response);

			return new EbayOrderModel(obj);
		}

		public EbayOrdersListModel GetOrders(int limit, int page)
		{
			int offset = (page - 1) * 50;
			var response = _service.GetRequest("/sell/fulfillment/v1/order" +
				//$"?filter=orderfulfillmentstatus:{{{status}}}" + {FULFILLED|IN_PROGRESS} {NOT_STARTED|IN_PROGRESS}
				$"?limit={limit}" +
				$"&offset={offset}");

			var obj = JsonConvert.DeserializeObject<GetOrdersResponseModel>(response);

			var model = new EbayOrdersListModel(obj, page, limit);

			return model;
		}

		public object GetItemInfo(string legacyItemId, string legacyVariationId)
		{
			throw new NotImplementedException();
		}

		public GetItemResponse GetItem(string legacyItemId, string legacyVariationId)
		{
			if (string.IsNullOrEmpty(legacyVariationId))
			{
				legacyVariationId = "0";
			}

			var response = _service.GetRequest($"https://api.ebay.com/buy/browse/v1/item/v1|{legacyItemId}|{legacyVariationId}");

			var obj = JsonConvert.DeserializeObject<GetItemResponse>(response);

			return obj;
		}

		public string GetVariant(string legacyItemId, string legacyVariationId)
		{
			var item = GetItem(legacyItemId, legacyVariationId);

			foreach(var aspect in item.localizedAspects)
			{
				if (aspect.name == "Character")
				{
					return aspect.value;
				}
			}

			return "";
		}
	}
}
