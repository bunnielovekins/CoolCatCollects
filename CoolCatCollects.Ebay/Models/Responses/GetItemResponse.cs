using System;

namespace CoolCatCollects.Ebay.Models.Responses
{
	public class GetItemResponse
	{
		public string itemId { get; set; }
		public string sellerItemRevision { get; set; }
		public string title { get; set; }
		public string shortDescription { get; set; }
		public PriceValue price { get; set; }
		public string categoryPath { get; set; }
		public string condition { get; set; }
		public string conditionId { get; set; }
		public Itemlocation itemLocation { get; set; }
		public Image image { get; set; }
		public Additionalimage[] additionalImages { get; set; }
		public string brand { get; set; }
		public Seller seller { get; set; }
		public string gtin { get; set; }
		public Estimatedavailability[] estimatedAvailabilities { get; set; }
		public Shippingoption[] shippingOptions { get; set; }
		public Shiptolocations shipToLocations { get; set; }
		public Returnterms returnTerms { get; set; }
		public Tax[] taxes { get; set; }
		public Localizedaspect[] localizedAspects { get; set; }
		public bool topRatedBuyingExperience { get; set; }
		public string[] buyingOptions { get; set; }
		public string itemWebUrl { get; set; }
		public string description { get; set; }
		public Primaryitemgroup primaryItemGroup { get; set; }
		public bool enabledForGuestCheckout { get; set; }
		public bool eligibleForInlineCheckout { get; set; }
		public string legacyItemId { get; set; }
		public bool adultOnly { get; set; }
		public string categoryId { get; set; }
	}

	public class Itemlocation
	{
		public string city { get; set; }
		public string postalCode { get; set; }
		public string country { get; set; }
	}

	public class Image
	{
		public string imageUrl { get; set; }
	}

	public class Seller
	{
		public string username { get; set; }
		public string feedbackPercentage { get; set; }
		public int feedbackScore { get; set; }
		public Sellerlegalinfo sellerLegalInfo { get; set; }
	}

	public class Sellerlegalinfo
	{
		public Sellerprovidedlegaladdress sellerProvidedLegalAddress { get; set; }
	}

	public class Sellerprovidedlegaladdress
	{
		public string addressLine1 { get; set; }
		public string city { get; set; }
		public string stateOrProvince { get; set; }
		public string postalCode { get; set; }
		public string country { get; set; }
		public string countryName { get; set; }
	}

	public class Shiptolocations
	{
		public Regionincluded[] regionIncluded { get; set; }
		public Regionexcluded[] regionExcluded { get; set; }
	}

	public class Regionincluded
	{
		public string regionName { get; set; }
		public string regionType { get; set; }
	}

	public class Regionexcluded
	{
		public string regionName { get; set; }
		public string regionType { get; set; }
	}

	public class Returnterms
	{
		public bool returnsAccepted { get; set; }
		public string returnShippingCostPayer { get; set; }
		public Returnperiod returnPeriod { get; set; }
	}

	public class Returnperiod
	{
		public int value { get; set; }
		public string unit { get; set; }
	}

	public class Primaryitemgroup
	{
		public string itemGroupId { get; set; }
		public string itemGroupType { get; set; }
		public string itemGroupHref { get; set; }
		public string itemGroupTitle { get; set; }
		public Itemgroupimage itemGroupImage { get; set; }
	}

	public class Itemgroupimage
	{
		public string imageUrl { get; set; }
	}

	public class Additionalimage
	{
		public string imageUrl { get; set; }
	}

	public class Estimatedavailability
	{
		public string[] deliveryOptions { get; set; }
		public string estimatedAvailabilityStatus { get; set; }
		public int estimatedAvailableQuantity { get; set; }
		public int estimatedSoldQuantity { get; set; }
	}

	public class Shippingoption
	{
		public string shippingServiceCode { get; set; }
		public string shippingCarrierCode { get; set; }
		public string type { get; set; }
		public PriceValue shippingCost { get; set; }
		public int quantityUsedForEstimate { get; set; }
		public DateTime minEstimatedDeliveryDate { get; set; }
		public DateTime maxEstimatedDeliveryDate { get; set; }
		public PriceValue additionalShippingCostPerUnit { get; set; }
		public string shippingCostType { get; set; }
	}

	public class Tax
	{
		public Taxjurisdiction taxJurisdiction { get; set; }
		public string taxType { get; set; }
		public bool shippingAndHandlingTaxed { get; set; }
		public bool includedInPrice { get; set; }
		public bool ebayCollectAndRemitTax { get; set; }
	}

	public class Taxjurisdiction
	{
		public Region region { get; set; }
		public string taxJurisdictionId { get; set; }
	}

	public class Region
	{
		public string regionName { get; set; }
		public string regionType { get; set; }
	}

	public class Localizedaspect
	{
		public string type { get; set; }
		public string name { get; set; }
		public string value { get; set; }
	}

}
