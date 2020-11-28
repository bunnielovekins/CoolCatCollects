using CoolCatCollects.Bricklink.Models.Responses;
using CoolCatCollects.Data.Entities;
using CoolCatCollects.Models.Parts;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Linq;

namespace CoolCatCollects.Bricklink
{
	/// <summary>
	/// Wrapper for sending requests to Bricklink API
	/// </summary>
	public class BricklinkApiService
	{
		/// <summary>
		/// Sends a get request, returns result as a string
		/// </summary>
		/// <param name="url">Url to send to, relative to the BL base url</param>
		/// <returns>The result, as a string</returns>
		public string GetRequest(string url)
		{
			var client = new RestClient(Statics.ApiUrl)
			{
				Authenticator = OAuth1Authenticator.ForProtectedResource(
					Statics.ConsumerKey,
					Statics.ConsumerSecret,
					Statics.TokenValue,
					Statics.TokenSecret
				)
			};

			var request = new RestRequest(url);

			var response = client.Execute(request);

			return response.Content;
		}

		/// <summary>
		/// Sends a get request and deserialises the result
		/// </summary>
		/// <typeparam name="T">Type to deserialise to</typeparam>
		/// <param name="url">Url to send to, relative to the BL base url</param>
		/// <returns>The result as type T</returns>
		public T GetRequest<T>(string url) where T : class
		{
			var str = GetRequest(url);

			return JsonConvert.DeserializeObject<T>(str);
		}


		/// <summary>
		/// Given an existing inventory entity, updates it from the api
		/// </summary>
		/// <param name="partInv">An inventory entity</param>
		/// <returns>The same entity, updated</returns>
		public PartInventory UpdateInventoryFromApi(PartInventory partInv)
		{
			return
				UpdateInventoryFromApi(partInv.InventoryId, partInv) ??
				UpdateInventoryFromApi(partInv.Part.ItemType, partInv.Part.CategoryId, partInv.ColourId, partInv.Part.Number, partInv.Condition, partInv);
		}

		public PartInventoryModel UpdateInventoryModelFromApi(int inventoryId)
		{
			var partInv = new PartInventoryModel();

			var response = GetRequest<GetInventoryResponseModel>($"inventories/{inventoryId}");

			var item = response.data;

			if (item == null)
			{
				return null;
			}

			partInv.InventoryId = item.inventory_id;
			partInv.Quantity = item.quantity;
			partInv.MyPrice = decimal.Parse(item.unit_price);
			partInv.ColourId = item.color_id;
			partInv.ColourName = Statics.Colours[item.color_id].Name;
			partInv.Condition = item.new_or_used;
			partInv.Location = item.remarks;
			partInv.Description = item.description;
			partInv.Image = GetItemImage(item.item.type, item.item.no, item.color_id);

			if (string.IsNullOrEmpty(partInv.Location))
			{
				partInv.Location = item.description;
			}

			partInv.LastUpdated = DateTime.Now;

			partInv.Number = item.item.no;
			partInv.Name = item.item.name;
			partInv.ItemType = item.item.type;
			partInv.CategoryId = item.item.category_id;

			if (item.is_stock_room)
			{
				partInv.Location = "";
				partInv.Quantity = 0;
			}

			return partInv;
		}

		public Part RecoverPartFromPartInv(PartInventory partInv, Part part)
		{
			if (partInv == null)
			{
				return part;
			}

			var response = GetRequest<GetInventoryResponseModel>($"inventories/{partInv.InventoryId}");

			var number = response.data.item.no;
			var type = response.data.item.type;

			return UpdatePartFromApi(number, type, part);
		}

		public PartInventory UpdateInventoryFromApi(int inventoryId, PartInventory partInv = null)
		{
			if (partInv == null)
			{
				partInv = new PartInventory();
			}

			var response = GetRequest<GetInventoryResponseModel>($"inventories/{inventoryId}");

			var item = response.data;

			if (item == null)
			{
				return null;
			}

			partInv.InventoryId = item.inventory_id;
			partInv.Quantity = item.quantity;
			partInv.MyPrice = decimal.Parse(item.unit_price);
			partInv.ColourId = item.color_id;
			partInv.ColourName = Statics.Colours[item.color_id].Name;
			partInv.Condition = item.new_or_used;
			partInv.Location = item.remarks;
			partInv.Description = item.description;
			partInv.Image = GetItemImage(item.item.type, item.item.no, item.color_id);

			if (string.IsNullOrEmpty(partInv.Location))
			{
				partInv.Location = item.description;
			}

			if (item.is_stock_room)
			{
				partInv.Quantity = 0;
				partInv.Location = "";
			}

			partInv.LastUpdated = DateTime.Now;

			return partInv;
		}

		/// <summary>
		/// Gets an inventory item from the api, puts it into an entity
		/// </summary>
		/// <param name="type"></param>
		/// <param name="categoryId"></param>
		/// <param name="colourId"></param>
		/// <param name="number"></param>
		/// <param name="condition"></param>
		/// <param name="partInv"></param>
		/// <returns></returns>
		public PartInventory UpdateInventoryFromApi(string type, int categoryId, int colourId, string number, string condition = "N", PartInventory partInv = null, string description = "")
		{
			if (partInv == null)
			{
				partInv = new PartInventory();
			}

			var response = GetRequest<GetInventoriesResponseModel>($"inventories?item_type={type}&category_id={categoryId}&color_id={colourId}");

			var item = response.data.FirstOrDefault(x => x.item.no == number && x.new_or_used == condition && x.description == description);

			if (item == null)
			{
				return null;
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

			if (item.is_stock_room)
			{
				partInv.Quantity = 0;
				partInv.Location = "";
			}

			partInv.LastUpdated = DateTime.Now;

			return partInv;
		}

		/// <summary>
		/// Gets the image url for a part
		/// </summary>
		/// <param name="type">Type - PART, MINIFIG, etc.</param>
		/// <param name="number">Part Number</param>
		/// <param name="colourId">Colour Id</param>
		/// <returns>Image url</returns>
		public string GetItemImage(string type, string number, int colourId)
		{
			var response = GetRequest<GetItemImageResponse>($"/items/{type}/{number}/images/{colourId}");

			return response.data.thumbnail_url;
		}

		/// <summary>
		/// Gets a part from the API, returns it as a Part. No relation to the DB.
		/// </summary>
		/// <param name="part">A Part entity</param>
		/// <param name="number">Part Number</param>
		/// <param name="type">Type - PART, MINIFIG, etc.</param>
		/// <returns>A Part entity</returns>
		public Part UpdatePartFromApi(string number, string type, Part part = null)
		{
			if (part == null)
			{
				part = new Part();
			}

			var response = GetRequest<GetItemResponse>($"items/{type}/{number}");

			if (response.data == null)
			{
				return null;
			}

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

		/// <summary>
		/// Gets pricing from the api
		/// </summary>
		/// <param name="price">Entity</param>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="colourId"></param>
		/// <param name="condition"></param>
		/// <returns></returns>
		public PartPriceInfo UpdatePartPricingFromApi(string number, string type, int colourId, string condition = "N", PartPriceInfo price = null)
		{
			if (price == null)
			{
				price = new PartPriceInfo();
			}

			var response = GetRequest<GetPriceGuideResponse>($"items/{type}/{number}/price?guide_type=sold&currency_code=GBP&vat=Y&country_code=UK&new_or_used={condition}&color_id={colourId}");

			var loc = "";

			// Fall back to EU
			if (response.data.avg_price == "0.0000")
			{
				response = GetRequest<GetPriceGuideResponse>($"items/{type}/{number}/price?guide_type=sold&currency_code=GBP&vat=Y&region=europe&new_or_used={condition}&color_id={colourId}");
				loc = "(EU)";
			}

			// Fall back to world
			if (response.data.avg_price == "0.0000")
			{
				response = GetRequest<GetPriceGuideResponse>($"items/{type}/{number}/price?guide_type=sold&currency_code=GBP&vat=Y&new_or_used={condition}&color_id={colourId}");
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
	}
}
