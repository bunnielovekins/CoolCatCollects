using CoolCatCollects.Data.Entities;
using CoolCatCollects.Ebay.Models;
using CoolCatCollects.Ebay.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CoolCatCollects.Ebay
{
	public class eBayService
	{
		private readonly eBayApiService _service;
		private readonly eBayDataService _dataService;

		public eBayService()
		{
			_service = new eBayApiService();
			_dataService = new eBayDataService();
		}

		public EbayOrderModel GetOrder(string orderNumber)
		{
			var response = _service.GetRequest($"sell/fulfillment/v1/order/{orderNumber}");

			var obj = JsonConvert.DeserializeObject<GetOrderResponseModel>(response);

			var orderEntity = _dataService.AddOrder(obj);

			bool updated = false;
			foreach(var item in orderEntity.OrderItems.Cast<EbayOrderItem>())
			{
				if (!string.IsNullOrEmpty(item.Image))
				{
					continue;
				}

				updated = true;

				var itemFromApi = GetItem(item.LegacyItemId, item.LegacyVariationId);

				item.Image = itemFromApi.image.imageUrl;

				if (itemFromApi.localizedAspects != null && itemFromApi.localizedAspects.Any())
				{
					foreach (var aspect in itemFromApi.localizedAspects)
					{
						if (aspect.name == "Character")
						{
							item.CharacterName = aspect.value;
							break;
						}
					}
				}

				_dataService.UpdateOrderItem(item);
			}

			if (updated)
			{
				orderEntity = _dataService.AddOrder(obj);
			}

			return new EbayOrderModel(obj, orderEntity);
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
	}
}
