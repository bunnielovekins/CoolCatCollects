using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Bricklink.Models.Responses;
using CoolCatCollects.Data.Entities;
using CoolCatCollects.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Bricklink
{
	public class BricklinkDataService
	{
		private readonly BaseRepository<Part> _partrepo;
		private readonly PartInventoryRepository _partInventoryRepo;
		private readonly BaseRepository<PartPriceInfo> _partPricingRepo;
		private readonly BaseRepository<PartInventoryLocationHistory> _partLocationHistoryRepo;
		private readonly BricklinkApiService _api;

		public BricklinkDataService()
		{
			_partrepo = new BaseRepository<Part>();
			_partInventoryRepo = new PartInventoryRepository();
			_partPricingRepo = new BaseRepository<PartPriceInfo>();
			_partLocationHistoryRepo = new BaseRepository<PartInventoryLocationHistory>();
			_api = new BricklinkApiService();
		}

		public PartModel GetPartModel(string number, int colourId, string type, char condition = 'N',
			bool updateInv = false, bool updatePrice = false, bool updatePart = false,
			DateTime? updateInvDate = null)
		{
			var partInv = _partInventoryRepo.FindOne(x => x.Part.ItemType == type && x.Part.Number == number && x.ColourId == colourId);

			if (partInv == null)
			{
				var part = GetPart(number, type);

				partInv = new PartInventory();
				partInv = UpdateInventoryFromApi(partInv, type, part.CategoryId, colourId, number, condition == 'A' ? 'N' : condition);

				if (partInv.Quantity == 0 && condition == 'A')
				{
					var usedPart = UpdateInventoryFromApi(new PartInventory(), type, part.CategoryId, colourId, number, 'U');
					if (usedPart.Quantity > 0)
					{
						partInv = usedPart;
					}
				}

				if (partInv.ColourId != 0)
				{
					partInv.ColourName = Statics.Colours[partInv.ColourId].Name;
				}

				var partPrice = new PartPriceInfo();
				var pricing = UpdatePartPricingFromApi(partPrice, number, type, colourId, partInv.Condition);

				_partInventoryRepo.AddPartInv(ref partInv, ref partPrice, ref part);

				return new PartModel
				{
					Part = part,
					PartInventory = partInv,
					PartPriceInfo = pricing
				};
			}
			else
			{
				if (partInv.LastUpdated < DateTime.Now.AddDays(-1) || updateInv || updateInvDate.HasValue && updateInvDate > partInv.LastUpdated)
				{
					partInv = UpdateInventoryFromApi(partInv);
					_partInventoryRepo.Update(partInv);
				}

				var part = partInv.Part;
				if (part.LastUpdated < DateTime.Now.AddDays(-14) || updatePart)
				{
					part = UpdatePartFromApi(part, number, type);
					_partrepo.Update(part);
				}

				var pricing = partInv.Pricing;
				if (pricing.LastUpdated < DateTime.Now.AddDays(-14) || updatePrice)
				{
					pricing = UpdatePartPricingFromApi(pricing, number, type, colourId, condition);
					_partPricingRepo.Update(pricing);
				}

				return new PartModel
				{
					Part = part,
					PartInventory = partInv,
					PartPriceInfo = pricing
				};
			}
		}

		private string GetItemImage(string type, string number, int colourId)
		{
			var response = _api.GetRequest<GetItemImageResponse>($"/items/{type}/{number}/images/{colourId}");

			return response.data.thumbnail_url;
		}

		private Part GetPart(string number, string type)
		{
			var part = _partrepo.FindOne(x => x.ItemType == type && x.Number == number);

			if (part != null && part.LastUpdated >= DateTime.Now.AddDays(-14))
			{
				return part;
			}

			if (part == null)
			{
				part = new Part();
			}

			part = UpdatePartFromApi(part, number, type);
			return part;
		}

		private Part UpdatePartFromApi(Part part, string number, string type)
		{
			var response = _api.GetRequest<GetItemResponse>($"items/{type}/{number}");

			part.Number = response.data.no;
			part.Name = response.data.name;
			part.CategoryId = response.data.category_id;
			part.ImageUrl = response.data.image_url;
			part.ThumbnailUrl = response.data.thumbnail_url;
			part.Weight = response.data.weight;
			part.Description = response.data.description;
			part.LastUpdated = DateTime.Now;
			part.ItemType = response.data.type;

			return part;
		}

		#region api

		private PartInventory UpdateInventoryFromApi(PartInventory partInv)
		{
			return UpdateInventoryFromApi(partInv, partInv.Part.ItemType, partInv.Part.CategoryId, partInv.ColourId, partInv.Part.Number, partInv.Condition);
		}

		private PartPriceInfo UpdatePartPricingFromApi(PartPriceInfo price, string number, string type, int colourId, char condition = 'N')
		{
			var response = _api.GetRequest<GetPriceGuideResponse>($"items/{type}/{number}/price?guide_type=sold&currency_code=GBP&vat=Y&country_code=UK&new_or_used={condition}&color_id={colourId}");

			var loc = "";

			// Fall back to EU
			if (response.data.avg_price == "0.0000")
			{
				response = _api.GetRequest<GetPriceGuideResponse>($"items/{type}/{number}/price?guide_type=sold&currency_code=GBP&vat=Y&region=europe&new_or_used={condition}&color_id={colourId}");
				loc = "(EU)";
			}

			// Fall back to world
			if (response.data.avg_price == "0.0000")
			{
				response = _api.GetRequest<GetPriceGuideResponse>($"items/{type}/{number}/price?guide_type=sold&currency_code=GBP&vat=Y&new_or_used={condition}&color_id={colourId}");
				loc = "(World)";
			}

			if (decimal.TryParse(response.data.avg_price, out decimal tmp))
			{
				price.AveragePrice = tmp;
			}
			else
			{
				price.AveragePrice = 0;
			}
			price.AveragePriceLocation = loc;
			price.LastUpdated = DateTime.Now;

			return price;
		}

		public PartInventory UpdateInventoryFromApi(PartInventory partInv, int inventoryId, out string number, out string type)
		{
			var response = _api.GetRequest<GetInventoryResponseModel>("/inventories/" + inventoryId);

			var item = response.data;

			partInv.InventoryId = inventoryId;
			partInv.Quantity = item.quantity;
			partInv.MyPrice = decimal.Parse(item.unit_price);
			partInv.ColourId = item.color_id;
			partInv.ColourName = Statics.Colours[item.color_id].Name;
			partInv.Condition = item.new_or_used[0];
			partInv.Location = item.remarks;
			partInv.LastUpdated = DateTime.Now;
			partInv.Description = item.description;
			if (partInv.Part != null)
			{
				partInv.Image = GetItemImage(partInv.Part.ItemType, partInv.Part.Number, partInv.ColourId);
			}

			if (string.IsNullOrEmpty(partInv.Location))
			{
				partInv.Location = item.description;
			}

			number = item.item.no;
			type = item.item.type;

			return partInv;
		}

		private PartInventory UpdateInventoryFromApi(PartInventory partInv, string type, int categoryId, int colourId, string number, char condition = 'N')
		{
			var response = _api.GetRequest<GetInventoriesResponseModel>($"inventories?item_type={type}&category_id={categoryId}&color_id={colourId}");

			var item = response.data.FirstOrDefault(x => x.item.no == number);

			if (item == null)
			{
				partInv.InventoryId = 0;
				partInv.Quantity = 0;
				partInv.MyPrice = 0;
				partInv.ColourId = colourId;
				partInv.ColourName = Statics.Colours[colourId].Name;
				partInv.Condition = condition;
				partInv.Location = "";
				partInv.LastUpdated = DateTime.Now;
				partInv.Image = GetItemImage(type, number, colourId);

				return partInv;
			}

			partInv.InventoryId = item.inventory_id;
			partInv.Quantity = item.quantity;
			partInv.MyPrice = decimal.Parse(item.unit_price);
			partInv.ColourId = colourId;
			partInv.ColourName = Statics.Colours[colourId].Name;
			partInv.Condition = condition;
			partInv.Location = item.remarks;
			partInv.Description = item.description;
			partInv.Image = GetItemImage(type, number, colourId);

			if (string.IsNullOrEmpty(partInv.Location))
			{
				partInv.Location = item.description;
			}

			partInv.LastUpdated = DateTime.Now;

			return partInv;
		}

		#endregion
	}
}
