using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMove.JsonObjects.NearbySoldPrices
{

	public class NearbySoldPricesResponse
	{
		public bool isAuthenticated { get; set; }
		public Metadata metadata { get; set; }
		public Searchresult searchResult { get; set; }
	}

	public class Metadata
	{
		public string publicsiteUrl { get; set; }
		public string mediaServerUrl { get; set; }
		public long serverTimestamp { get; set; }
		public string deviceType { get; set; }
		public string deviceTypeForLazyLoad { get; set; }
		public Featureswitches featureSwitches { get; set; }
		public Mvts mvts { get; set; }
		public string correlationId { get; set; }
		public string locationSearchUrl { get; set; }
		public bool shouldRenderPartialPage { get; set; }
		public float rumTransactionSampleRate { get; set; }
		public string environment { get; set; }
		public string publiftAdsConfig { get; set; }
	}

	public class Featureswitches
	{
		public bool SoldPropertyWebSHOW_MORTGAGES_WIDGET { get; set; }
		public bool SoldPropertyWebSHOW_CX_CAVEAT { get; set; }
		public bool GlobalSwitchesCOOKIE_BANNER_IN_APP_ENABLED { get; set; }
		public bool GlobalSwitchesMANAGE_RMSESSIONID_ENABLED { get; set; }
		public bool SoldPropertyWebAPPLICATION_MANAGE_RMSESSIONID_ENABLED { get; set; }
		public bool SoldPropertyWebENABLED_SCRAPING_INTERCEPTOR { get; set; }
		public bool SoldPropertyWebENABLE_PUBLIFT_THIRD_PARTY_ADS { get; set; }
		public bool HousePricesFeatureSwitchesUSE_UPRN_BASED_INDEX { get; set; }
		public bool SoldPropertyWebENABLED_VWO { get; set; }
		public bool SoldPropertyWebEXTENDED_RESI_OUTCODE_LIST { get; set; }
		public bool CookieBarSwitchesBLOCK_UNKNOWN_COOKIES { get; set; }
		public bool CookieBarSwitchesBLOCK_UNKNOWN_COOKIES_V2 { get; set; }
		public bool CookieBarSwitchesENABLE_ONE_TRUST_COOKIE_SCRIPT { get; set; }
		public bool CookieBarSwitchesCLIENT_STORAGE_FILTERING_ENABLED { get; set; }
	}

	public class Mvts
	{
	}

	public class Searchresult
	{
		public int count { get; set; }
		public string metaTagDescription { get; set; }
		public Property1[] properties { get; set; }
		public Searchlocation searchLocation { get; set; }
		public Disclaimerdates disclaimerDates { get; set; }
		public Blurb blurb { get; set; }
		public Pagination pagination { get; set; }
		public Localinfo localInfo { get; set; }
		public Agent[] agents { get; set; }
		public int outcodeId { get; set; }
		public string outcode { get; set; }
		public bool showResiPotential { get; set; }
	}

	public class Searchlocation
	{
		public string displayName { get; set; }
		public string searchName { get; set; }
		public string locationType { get; set; }
		public int locationId { get; set; }
		public object encryptedUsrn { get; set; }
		public object outcodeId { get; set; }
	}

	public class Disclaimerdates
	{
		public Disclaimerdatesmap disclaimerDatesMap { get; set; }
	}

	public class Disclaimerdatesmap
	{
		public Landregistry landRegistry { get; set; }
	}

	public class Landregistry
	{
		public string earliestTransaction { get; set; }
		public string mostRecentTransaction { get; set; }
		public string lastLoadDate { get; set; }
	}

	public class Blurb
	{
		public object[] text { get; set; }
		public int numberOfProperties { get; set; }
		public int numberOfTransactions { get; set; }
		public string earliestTransactionDate { get; set; }
		public string latestTransactionDate { get; set; }
	}

	public class Pagination
	{
		public int current { get; set; }
		public int first { get; set; }
		public int last { get; set; }
		public int total { get; set; }
		public object sortBy { get; set; }
		public bool hasNext { get; set; }
	}

	public class Localinfo
	{
		public Nearbyhousepricelinks nearbyHousePriceLinks { get; set; }
		public Propertyvaluelinks propertyValueLinks { get; set; }
	}

	public class Nearbyhousepricelinks
	{
		public string title { get; set; }
		public Link[] links { get; set; }
	}

	public class Link
	{
		public string text { get; set; }
		public string url { get; set; }
	}

	public class Propertyvaluelinks
	{
		public string title { get; set; }
		public Link1[] links { get; set; }
	}

	public class Link1
	{
		public string text { get; set; }
		public string url { get; set; }
	}

	public class Property1
	{
		public string uuid { get; set; }
		public string encryptedUprn { get; set; }
		public string address { get; set; }
		public string propertyType { get; set; }
		public int? bedrooms { get; set; }
		public int? bathrooms { get; set; }
		public Imageinfo imageInfo { get; set; }
		public bool hasFloorPlan { get; set; }
		public Transaction[] transactions { get; set; }
		public Latesttransaction latestTransaction { get; set; }
		public Location location { get; set; }
		public string detailUrl { get; set; }
		public Staticmapurls staticMapUrls { get; set; }
	}

	public class Imageinfo
	{
		public string unsizedUrl { get; set; }
		public string imageUrl { get; set; }
		public string mediumImageUrl { get; set; }
		public int count { get; set; }
	}

	public class Latesttransaction
	{
		public string displayPrice { get; set; }
		public string dateSold { get; set; }
		public string tenure { get; set; }
		public bool newBuild { get; set; }
	}

	public class Location
	{
		public float lat { get; set; }
		public float lng { get; set; }
	}

	public class Staticmapurls
	{
		public string staticMapImgUrlMobile { get; set; }
		public string staticMapImgUrlDesktop { get; set; }
		public string staticMapImgUrlApp { get; set; }
	}

	public class Transaction
	{
		public string displayPrice { get; set; }
		public string dateSold { get; set; }
		public string tenure { get; set; }
		public bool newBuild { get; set; }
	}

	public class Agent
	{
		public int customerId { get; set; }
		public int branchId { get; set; }
		public string name { get; set; }
		public string displayAddress { get; set; }
		public string logoUrl { get; set; }
		public string tradingName { get; set; }
		public float latitude { get; set; }
		public float longitude { get; set; }
	}
}
