namespace CoolCatCollects.Core
{
	public static class PostageHelper
	{
		public static string FriendlyPostageName(string method)
		{
			switch (method)
			{
				case "UK_RoyalMailSecondClassStandard":
					return "RM 48";
				case "UK_RoyalMailFirstClassStandard":
					return "RM 24";
				case "UK_RoyalMailAirmailInternational":
					return "RM Intl Standard";
				case "UK_eBayDeliveryPacklinkIntl":
					return "eBay Packlink International";
				case "UK_RoyalMailSecondClassRecorded":
					return "RM Second Class Recorded";
				case "UK_RoyalMailFirstClassRecorded":
					return "RM First Class Recorded";
				case "UK_myHermesDoorToDoorService":
					return "MyHermes";
				case "RoyalMail - Standard Parcel":
					return "RM Intl Standard - Parcel";
				case "RoyalMail - Standard Large Letter":
					return "RM Intl Standard - Large Letter";
				case "RoyalMail - Standard 2nd Class Large Letter":
					return "RM 48 - Large Letter";
				case "RoyalMail - Standard 2nd Class Small Parcel":
					return "RM 48 - Small Parcel";
				case "RoyalMail - Standard 1st Class Large Letter":
					return "RM 24 - Large Letter";
				case "RoyalMail - Standard 1st Class Small Parcel":
					return "RM 24 - Small Parcel";
			}

			return method;
		}
	}
}
