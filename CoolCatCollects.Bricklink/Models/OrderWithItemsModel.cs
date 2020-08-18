using CoolCatCollects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CoolCatCollects.Bricklink.Models
{
	public class OrderWithItemsModel
	{
		public OrderWithItemsModel()
		{

		}

		public OrderWithItemsModel(GetOrderResponseModel order, GetOrderItemsResponseModel orderItems)
		{
			var data = order.data;
			BuyerName = data.shipping.address.name.full;
			UserName = data.buyer_name;
			ShippingMethod = data.shipping.method;
			OrderTotal = data.cost.subtotal;

			Buyer = new Buyer(data.shipping.address);
			OrderNumber = data.order_id.ToString();
			OrderDate = data.date_ordered.ToString("yyyy-MM-dd");
			OrderPaid = data.payment.date_paid.ToString("yyyy-MM-dd");
			SubTotal = StaticFunctions.FormatCurrencyStr(data.cost.subtotal);
			ServiceCharge = StaticFunctions.FormatCurrencyStr(data.cost.etc1);
			Coupon = StaticFunctions.FormatCurrencyStr(data.cost.coupon);
			PostagePackaging = StaticFunctions.FormatCurrencyStr(data.cost.shipping);
			Total = StaticFunctions.FormatCurrencyStr(data.cost.grand_total);

			Items = orderItems.data
				.SelectMany(x => x)
				.Select(x => new OrderItemModel(x))
				.OrderBy(x => x.Condition)
				.ThenBy(x => x.RemarkLetter3)
				.ThenBy(x => x.RemarkLetter2)
				.ThenBy(x => x.RemarkLetter1)
				.ThenBy(x => x.RemarkNumber)
				.ThenBy(x => x.Colour)
				.ThenBy(x => x.Name);

			Messages = new List<BricklinkMessage>();
		}

		public string BuyerName { get; set; }
		public string UserName { get; set; }
		public string ShippingMethod { get; set; }
		public string OrderTotal { get; set; }
		public IEnumerable<OrderItemModel> Items { get; set; }

		public Buyer Buyer { get; set; }
		public string OrderNumber { get; set; }
		public string OrderDate { get; set; }
		public string OrderPaid { get; set; }
		public string SubTotal { get; set; }
		public string ServiceCharge { get; set; }
		public string Coupon { get; set; }
		public string PostagePackaging { get; set; }
		public string Total { get; set; }
		public string OrderRemarks { get; set; }
		public IEnumerable<BricklinkMessage> Messages { get; set; }
	}

	public class BricklinkMessage
	{
		public string InOrOut { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public DateTime Date { get; set; }
	}

	public class Buyer
	{
		public Buyer(BricklinkAddress address)
		{
			Name = address.name.full;
			Address1 = address.address1;
			Address2 = address.address2;
			PostCode = address.postal_code;
			Country = address.country_code;
			City = address.city;
			FullAddress = address.full;
		}

		public Buyer()
		{

		}

		public string Name { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string PostCode { get; set; }
		public string Country { get; set; }
		public string City { get; set; }
		public string FullAddress { get; set; }
	}

	public class OrderItemModel
	{
		public OrderItemModel()
		{

		}

		public OrderItemModel(OrderItemResponseModel item)
		{
			InventoryId = item.inventory_id.ToString();
			Name = HttpUtility.HtmlDecode(item.item.name);
			Condition = item.new_or_used == "N" ? "New" : "Used";
			Colour = item.color_name;
			if (Colour == "(Not Applicable)")
			{
				Colour = "";
			}
			Remarks = item.remarks;
			if (string.IsNullOrEmpty(Remarks))
			{
				Remarks = item.description;
			}
			Quantity = item.quantity;
			UnitPrice = decimal.Parse(item.unit_price_final);
			TotalPrice = UnitPrice * Quantity;
			Description = item.description;
			Type = item.item.type;
			Weight = item.weight;
			ItemsRemaining = 0;

			FillRemarks();
		}

		/// <summary>
		/// Fills in some fields to be used for ordering - first letter, second letter, number of remarks
		/// </summary>
		public void FillRemarks()
		{
			if (!string.IsNullOrEmpty(Remarks))
			{
				var regex = new Regex("(\\D*)(\\d*)");
				var match = regex.Match(Remarks);
				if (match.Success)
				{
					if (match.Groups.Count > 0 && !string.IsNullOrEmpty(match.Groups[1].Value))
					{
						RemarkLetter1 = match.Groups[1].Value[0];
						RemarkLetter2 = match.Groups[1].Value.Length > 1 ? match.Groups[1].Value[1] : ' ';
						RemarkLetter3 = match.Groups[1].Value.Length > 2 ? match.Groups[1].Value[2] : ' ';
					}
					if (match.Groups.Count > 1)
					{
						if (int.TryParse(match.Groups[2].Value, out int tmpNum))
						{
							RemarkNumber = tmpNum;
						}
						else
						{
							RemarkNumber = 0;
						}
					}
				}
			}
		}

		public string InventoryId { get; set; }
		public string Type { get; set; }
		public string Weight { get; set; }
		public int ItemsRemaining { get; set; }
		public string Name { get; set; }
		public string Condition { get; set; }
		public string Colour { get; set; }
		public string Remarks { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal TotalPrice { get; set; }
		public string Image { get; set; }
		public char RemarkLetter1 { get; set; }
		public char RemarkLetter2 { get; set; }
		public char RemarkLetter3 { get; set; }

		public int RemarkNumber { get; set; }
		public string Description { get; set; }
	}
}
