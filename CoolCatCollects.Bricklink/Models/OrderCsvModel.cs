﻿using System;
using System.ComponentModel;

namespace CoolCatCollects.Bricklink.Models
{
	/// <summary>
	/// Model for making into the Royal mail CSV
	/// </summary>
	public class OrderCsvModel
	{
		public OrderCsvModel()
		{

		}

		public OrderCsvModel(GetOrderResponseModel model)
		{
			var data = model.data;

			if (data.shipping == null)
			{
				Name = "ERROR: Shipping was null!";
				return;
			}

			if (data.shipping.address == null)
			{
				Name = "ERROR: Shipping address was null!";
				return;
			}

			Name = data.shipping.address.name.full;
			Address1 = data.shipping.address.address1;
			Address2 = data.shipping.address.address2;
			AddressCity = data.shipping.address.city;
			AddressPostcode = data.shipping.address.postal_code;
			AddressCounty = data.shipping.address.state;
			AddressCountry = data.shipping.address.country_code;
			OrderReference = data.order_id.ToString();
			OrderValue = Core.StaticFunctions.FormatCurrency(data.cost?.subtotal ?? "0").ToString();
			ShippingCost = Core.StaticFunctions.FormatCurrency(data.cost?.shipping ?? "0").ToString();
			Weight = formatWeight(data.total_weight);
			ShippingMethod = data.shipping.method;
			PackageSize = GetPackageSize();

			ProductName = "Mixed Lego (No Batteries)";
			UnitPrice = Core.StaticFunctions.FormatCurrency(data.cost?.subtotal ?? "0").ToString();
			Quantity = "1";
			UnitWeight = Weight;
		}

		private string GetPackageSize()
		{
			if (string.IsNullOrWhiteSpace(ShippingMethod))
			{
				return "";
			}

			if (ShippingMethod.ToLower().Contains("large letter"))
			{
				return "large letter";
			}
			else if (ShippingMethod.ToLower().Contains("parcel"))
			{
				return "small parcel";
			}
			else if (ShippingMethod.ToLower().Contains("letter"))
			{
				return "letter";
			}

			return "";
		}

		/// <summary>
		/// Round the weight up to the nearest threshold. Not actually used by RM, only used as a guide for us.
		/// </summary>
		/// <param name="weight"></param>
		/// <returns></returns>
		private string formatWeight(string weight)
		{
			var d = decimal.Parse(weight);
			
			if (d <= 80)
			{
				return "0.1";
			}
			if (d <= 230)
			{
				return "0.25";
			}
			if (d <= 480)
			{
				return "0.5";
			}
			if (d <= 730)
			{
				return "0.75";
			}
			if (d <= 980)
			{
				return "1";
			}
			if (d <= 1230)
			{
				return "1.25";
			}
			if (d <= 1480)
			{
				return "1.5";
			}
			if (d <= 1730)
			{
				return "1.75";
			}
			if (d <= 1980)
			{
				return "2";
			}

			return (d / 1000).ToString();
		}

		[DisplayName("Name")]
		public string Name { get; set; }
		[DisplayName("Address Line 1")]
		public string Address1 { get; set; }
		[DisplayName("Address Line 2")]
		public string Address2 { get; set; }
		[DisplayName("Address City")]
		public string AddressCity { get; set; }
		[DisplayName("Address Postcode")]
		public string AddressPostcode { get; set; }
		[DisplayName("Address County")]
		public string AddressCounty { get; set; }
		[DisplayName("Address Country")]
		public string AddressCountry { get; set; }
		[DisplayName("Order Reference")]
		public string OrderReference { get; set; }
		[DisplayName("Order Value")]
		public string OrderValue { get; set; }
		[DisplayName("Shipping Cost")]
		public string ShippingCost { get; set; }
		[DisplayName("Weight")]
		public string Weight { get; set; }
		[DisplayName("Shipping Method")]
		public string ShippingMethod { get; set; }
		public string PackageSize { get; set; }
		[DisplayName("Product Name")]
		public string ProductName { get; set; }
		[DisplayName("Unit Price")]
		public string UnitPrice { get; set; }
		public string Quantity { get; set; }
		[DisplayName("Unit Weight")]
		public string UnitWeight { get; set; }
	}
}
