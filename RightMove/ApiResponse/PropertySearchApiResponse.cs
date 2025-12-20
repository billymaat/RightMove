using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMove.ApiResponse
{

	public class PropertySearchApiResponse
	{
		public string countryCode { get; set; }
		public int countryId { get; set; }
		public Dfpmodel dfpModel { get; set; }
		public string formattedExchangeRateDate { get; set; }
		public Location location { get; set; }
		public Noresultsmodel noResultsModel { get; set; }
		public Pagination pagination { get; set; }
		public Property1[] properties { get; set; }
		public string resultCount { get; set; }
		public Searchparameters searchParameters { get; set; }
		public string searchParametersDescription { get; set; }
		public Sidebarmodel sidebarModel { get; set; }
		public int keywordCount { get; set; }
		public string pageTitle { get; set; }
		public string metaDescription { get; set; }
		public Seomodel seoModel { get; set; }
		public long timestamp { get; set; }
		public object urlPath { get; set; }
		public string termsOfUse { get; set; }
		public string staticMapUrl { get; set; }
		public string listViewUrl { get; set; }
		public string mapViewUrl { get; set; }
	}

	public class Dfpmodel
	{
		public Sidebarslot[] sidebarSlots { get; set; }
		public Targeting[] targeting { get; set; }
	}

	public class Sidebarslot
	{
		public string id { get; set; }
		public string adUnitPath { get; set; }
		public int[][] sizes { get; set; }
		public object[] mappings { get; set; }
	}

	public class Targeting
	{
		public string key { get; set; }
		public string value { get; set; }
	}

	public class Location
	{
		public int id { get; set; }
		public string displayName { get; set; }
		public string shortDisplayName { get; set; }
		public string locationType { get; set; }
		public string listingCurrency { get; set; }
		public Geometry geometry { get; set; }
		public Encodedgeometry encodedGeometry { get; set; }
	}

	public class Geometry
	{
		public string type { get; set; }
		public float[][][] coordinates { get; set; }
	}

	public class Encodedgeometry
	{
		public string encodedPolygon { get; set; }
	}

	public class Noresultsmodel
	{
		public object[] suggestionPods { get; set; }
		public object intelligentSuggestion { get; set; }
	}

	public class Pagination
	{
		public int total { get; set; }
		public Option[] options { get; set; }
		public string first { get; set; }
		public string last { get; set; }
		public string next { get; set; }
		public string page { get; set; }
	}

	public class Option
	{
		public string value { get; set; }
		public string description { get; set; }
	}

	public class Searchparameters
	{
		public string locationIdentifier { get; set; }
		public string maxBedrooms { get; set; }
		public string minBedrooms { get; set; }
		public string maxPrice { get; set; }
		public string minPrice { get; set; }
		public string numberOfPropertiesPerPage { get; set; }
		public string radius { get; set; }
		public string sortType { get; set; }
		public string index { get; set; }
		public object[] propertyTypes { get; set; }
		public object[] tenureTypes { get; set; }
		public string viewType { get; set; }
		public object[] mustHave { get; set; }
		public object[] dontShow { get; set; }
		public object[] furnishTypes { get; set; }
		public string channel { get; set; }
		public string areaSizeUnit { get; set; }
		public string currencyCode { get; set; }
		public object[] keywords { get; set; }
		public object[] tags { get; set; }
	}

	public class Sidebarmodel
	{
		public Soldhousepriceslinks soldHousePricesLinks { get; set; }
		public Relatedhousesearches relatedHouseSearches { get; set; }
		public Relatedflatsearches relatedFlatSearches { get; set; }
		public Relatedpopularsearches relatedPopularSearches { get; set; }
		public object relatedRegionsSearches { get; set; }
		public Relatedsuggestedsearches relatedSuggestedSearches { get; set; }
		public Channelswitchlink channelSwitchLink { get; set; }
		public object relatedStudentLinks { get; set; }
		public object branchMPU { get; set; }
		public object countryGuideMPU { get; set; }
		public Suggestedlinks suggestedLinks { get; set; }
	}

	public class Soldhousepriceslinks
	{
		public string heading { get; set; }
		public string subHeading { get; set; }
		public Model[] model { get; set; }
		public object headingLink { get; set; }
	}

	public class Model
	{
		public string text { get; set; }
		public string url { get; set; }
		public bool noFollow { get; set; }
	}

	public class Relatedhousesearches
	{
		public string heading { get; set; }
		public object subHeading { get; set; }
		public Model1[] model { get; set; }
		public object headingLink { get; set; }
	}

	public class Model1
	{
		public string text { get; set; }
		public string url { get; set; }
		public bool noFollow { get; set; }
	}

	public class Relatedflatsearches
	{
		public string heading { get; set; }
		public object subHeading { get; set; }
		public Model2[] model { get; set; }
		public Headinglink headingLink { get; set; }
	}

	public class Headinglink
	{
		public string text { get; set; }
		public string url { get; set; }
		public bool noFollow { get; set; }
	}

	public class Model2
	{
		public string text { get; set; }
		public string url { get; set; }
		public bool noFollow { get; set; }
	}

	public class Relatedpopularsearches
	{
		public string heading { get; set; }
		public object subHeading { get; set; }
		public Model3[] model { get; set; }
		public object headingLink { get; set; }
	}

	public class Model3
	{
		public string text { get; set; }
		public string url { get; set; }
		public bool noFollow { get; set; }
	}

	public class Relatedsuggestedsearches
	{
		public string heading { get; set; }
		public object subHeading { get; set; }
		public Model4[] model { get; set; }
		public object headingLink { get; set; }
	}

	public class Model4
	{
		public string text { get; set; }
		public string url { get; set; }
		public bool noFollow { get; set; }
	}

	public class Channelswitchlink
	{
		public string heading { get; set; }
		public object subHeading { get; set; }
		public Model5[] model { get; set; }
		public object headingLink { get; set; }
	}

	public class Model5
	{
		public string text { get; set; }
		public string url { get; set; }
		public bool noFollow { get; set; }
	}

	public class Suggestedlinks
	{
		public string heading { get; set; }
		public object subHeading { get; set; }
		public Model6[] model { get; set; }
		public object headingLink { get; set; }
	}

	public class Model6
	{
		public string text { get; set; }
		public string url { get; set; }
		public bool noFollow { get; set; }
	}

	public class Seomodel
	{
		public string canonicalUrl { get; set; }
		public string metaRobots { get; set; }
	}

	public class Property1
	{
		public int id { get; set; }
		public int? bedrooms { get; set; }
		public int? bathrooms { get; set; }
		public int? numberOfImages { get; set; }
		public int? numberOfFloorplans { get; set; }
		public int? numberOfVirtualTours { get; set; }
		public string? summary { get; set; }
		public string displayAddress { get; set; }
		public string? countryCode { get; set; }
		public Location1 location { get; set; }
		public Image1[] images { get; set; }
		public string propertySubType { get; set; }
		public Tenure tenure { get; set; }
		public object letAvailableDate { get; set; }
		public Listingupdate listingUpdate { get; set; }
		public Price price { get; set; }
		public bool premiumListing { get; set; }
		public bool featuredProperty { get; set; }
		public bool commercialSearchProminenceSelected { get; set; }
		public Customer customer { get; set; }
		public object distance { get; set; }
		public string transactionType { get; set; }
		public Productlabel productLabel { get; set; }
		public bool commercial { get; set; }
		public bool development { get; set; }
		public bool residential { get; set; }
		public bool students { get; set; }
		public bool auction { get; set; }
		public bool feesApply { get; set; }
		public object feesApplyText { get; set; }
		public string displaySize { get; set; }
		public bool showOnMap { get; set; }
		public string propertyUrl { get; set; }
		public string contactUrl { get; set; }
		public object staticMapUrl { get; set; }
		public string channel { get; set; }
		public DateTime firstVisibleDate { get; set; }
		public object[] keywords { get; set; }
		public object[] tags { get; set; }
		public string keywordMatchType { get; set; }
		public bool saved { get; set; }
		public bool hidden { get; set; }
		public bool onlineViewingsAvailable { get; set; }
		public Lozengemodel lozengeModel { get; set; }
		public Streetview streetView { get; set; }
		public object enquiredTimestamp { get; set; }
		public DateTime updateDate { get; set; }
		public object enquiryAddedTimestamp { get; set; }
		public object enquiryCalledTimestamp { get; set; }
		public object reviews { get; set; }
		public Keyfeature[] keyFeatures { get; set; }
		public bool enhancedListing { get; set; }
		public Propertyimages propertyImages { get; set; }
		public string formattedBranchName { get; set; }
		public string addedOrReduced { get; set; }
		public string formattedDistance { get; set; }
		public string heading { get; set; }
		public string propertyTypeFullDescription { get; set; }
		public string displayStatus { get; set; }
		public bool isRecent { get; set; }
		public bool hasBrandPlus { get; set; }
	}

	public class Location1
	{
		public float latitude { get; set; }
		public float longitude { get; set; }
	}

	public class Tenure
	{
		public string tenureType { get; set; }
	}

	public class Listingupdate
	{
		public string listingUpdateReason { get; set; }
		public DateTime listingUpdateDate { get; set; }
	}

	public class Price
	{
		public int amount { get; set; }
		public string frequency { get; set; }
		public string currencyCode { get; set; }
		public Displayprice[] displayPrices { get; set; }
	}

	public class Displayprice
	{
		public string displayPrice { get; set; }
		public string displayPriceQualifier { get; set; }
	}

	public class Customer
	{
		public int branchId { get; set; }
		public string brandPlusLogoURI { get; set; }
		public string contactTelephone { get; set; }
		public string branchDisplayName { get; set; }
		public string branchName { get; set; }
		public string brandTradingName { get; set; }
		public string branchLandingPageUrl { get; set; }
		public bool development { get; set; }
		public string mediaServerUrl { get; set; }
		public bool showReducedProperties { get; set; }
		public bool hasBrandPlus { get; set; }
		public bool commercial { get; set; }
		public bool showOnMap { get; set; }
		public bool enhancedListing { get; set; }
		public object developmentContent { get; set; }
		public bool buildToRent { get; set; }
		public object[] buildToRentBenefits { get; set; }
		public DateTime updateDate { get; set; }
		public string brandPlusLogoUrl { get; set; }
		public string primaryBrandColour { get; set; }
	}

	public class Productlabel
	{
		public string productLabelText { get; set; }
		public bool spotlightLabel { get; set; }
	}

	public class Lozengemodel
	{
		public Matchinglozenge[] matchingLozenges { get; set; }
	}

	public class Matchinglozenge
	{
		public string type { get; set; }
		public int priority { get; set; }
	}

	public class Streetview
	{
		public bool showStreetView { get; set; }
	}

	public class Propertyimages
	{
		public Image[] images { get; set; }
		public string mainImageSrc { get; set; }
		public string mainMapImageSrc { get; set; }
	}

	public class Image
	{
		public string srcUrl { get; set; }
		public string url { get; set; }
		public string caption { get; set; }
	}

	public class Image1
	{
		public string srcUrl { get; set; }
		public string url { get; set; }
		public string caption { get; set; }
	}

	public class Keyfeature
	{
		public int order { get; set; }
		public string description { get; set; }
		public string htmlDescription { get; set; }
	}

}
