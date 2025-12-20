using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove.JsonObjects
{
	public class Rootobject
	{
		public Propertydata propertyData { get; set; }
		public Metadata metadata { get; set; }
		public bool isAuthenticated { get; set; }
		public Analyticsinfo analyticsInfo { get; set; }
	}

	public class Propertydata
	{
		public string id { get; set; }
		public Status status { get; set; }
		public Text text { get; set; }
		public Prices prices { get; set; }
		public Address address { get; set; }
		public string[] keyFeatures { get; set; }
		public Image[] images { get; set; }
		public object[] floorplans { get; set; }
		public Virtualtour[] virtualTours { get; set; }
		public Customer customer { get; set; }
		public Industryaffiliation[] industryAffiliations { get; set; }
		public Room[] rooms { get; set; }
		public Location location { get; set; }
		public Streetview streetView { get; set; }
		public object[] nearestAirports { get; set; }
		public Neareststation[] nearestStations { get; set; }
		public bool showSchoolInfo { get; set; }
		public object countryGuide { get; set; }
		public string channel { get; set; }
		public Propertyurls propertyUrls { get; set; }
		public object[] sizings { get; set; }
		public Brochure[] brochures { get; set; }
		public object[] epcGraphs { get; set; }
		public int bedrooms { get; set; }
		public int bathrooms { get; set; }
		public string transactionType { get; set; }
		public object[] tags { get; set; }
		public Misinfo misInfo { get; set; }
		public Dfpadinfo dfpAdInfo { get; set; }
		public Staticmapimgurls staticMapImgUrls { get; set; }
		public Listinghistory listingHistory { get; set; }
		public object feesApply { get; set; }
		public Broadband broadband { get; set; }
		public Contactinfo contactInfo { get; set; }
		public object lettings { get; set; }
		public Inforeelitem[] infoReelItems { get; set; }
		public Mortgagecalculator mortgageCalculator { get; set; }
		public Tenure tenure { get; set; }
		public string soldPropertyType { get; set; }
		public object auctionProvider { get; set; }
	}

	public class Status
	{
		public bool published { get; set; }
		public bool archived { get; set; }
	}

	public class Text
	{
		public string description { get; set; }
		public string propertyPhrase { get; set; }
		public string disclaimer { get; set; }
		public object auctionFeesDisclaimer { get; set; }
		public object guidePriceDisclaimer { get; set; }
		public object reservePriceDisclaimer { get; set; }
		public string staticMapDisclaimerText { get; set; }
		public string newHomesBrochureDisclaimer { get; set; }
		public string shareText { get; set; }
		public string shareDescription { get; set; }
		public string pageTitle { get; set; }
	}

	public class Prices
	{
		public string primaryPrice { get; set; }
		public object secondaryPrice { get; set; }
		public string displayPriceQualifier { get; set; }
		public object exchangeRate { get; set; }
	}

	public class Address
	{
		public string displayAddress { get; set; }
		public string countryCode { get; set; }
		public int deliveryPointId { get; set; }
		public string ukCountry { get; set; }
	}

	public class Customer
	{
		public int branchId { get; set; }
		public string branchName { get; set; }
		public string branchDisplayName { get; set; }
		public string companyName { get; set; }
		public string displayAddress { get; set; }
		public string logoPath { get; set; }
		public Customerdescription customerDescription { get; set; }
		public string bannerAd { get; set; }
		public string mpuAd { get; set; }
		public string customerProfileUrl { get; set; }
		public string customerBannerAdProfileUrl { get; set; }
		public string customerMpuAdProfileUrl { get; set; }
		public string customerPropertiesUrl { get; set; }
		public bool isNewHomeDeveloper { get; set; }
		public object spotlight { get; set; }
		public bool showBrochureLeadModal { get; set; }
		public Developmentinfo developmentInfo { get; set; }
	}

	public class Customerdescription
	{
		public string truncatedDescriptionHTML { get; set; }
		public bool isTruncated { get; set; }
	}

	public class Developmentinfo
	{
		public object sitePlanUri { get; set; }
		public object[] micrositeFeatures { get; set; }
	}

	public class Location
	{
		public float latitude { get; set; }
		public float longitude { get; set; }
		public int circleRadiusOnMap { get; set; }
		public int zoomLevel { get; set; }
		public string pinType { get; set; }
		public bool showMap { get; set; }
	}

	public class Streetview
	{
		public object heading { get; set; }
		public object pitch { get; set; }
		public object zoom { get; set; }
		public float latitude { get; set; }
		public float longitude { get; set; }
	}

	public class Propertyurls
	{
		public string similarPropertiesUrl { get; set; }
		public string nearbySoldPropertiesUrl { get; set; }
	}

	public class Misinfo
	{
		public int branchId { get; set; }
		public object offerAdvertStampTypeId { get; set; }
		public bool premiumDisplay { get; set; }
		public object premiumDisplayStampId { get; set; }
		public bool brandPlus { get; set; }
		public bool featuredProperty { get; set; }
	}

	public class Dfpadinfo
	{
		public string channel { get; set; }
		public Targeting[] targeting { get; set; }
	}

	public class Targeting
	{
		public string key { get; set; }
		public string[] value { get; set; }
	}

	public class Staticmapimgurls
	{
		public string staticMapImgUrlMobile { get; set; }
		public string staticMapImgUrlTablet { get; set; }
		public string staticMapImgUrlDesktopSmall { get; set; }
		public string staticMapImgUrlDesktopLarge { get; set; }
	}

	public class Listinghistory
	{
		public string listingUpdateReason { get; set; }
	}

	public class Broadband
	{
		public string disclaimer { get; set; }
		public string broadbandCheckerUrl { get; set; }
	}

	public class Contactinfo
	{
		public string contactMethod { get; set; }
		public Telephonenumbers telephoneNumbers { get; set; }
	}

	public class Telephonenumbers
	{
		public string localNumber { get; set; }
		public object internationalNumber { get; set; }
		public object disclaimerText { get; set; }
		public object disclaimerTitle { get; set; }
		public object disclaimerDescription { get; set; }
	}

	public class Mortgagecalculator
	{
		public int price { get; set; }
		public string propertyTypeAlias { get; set; }
	}

	public class Tenure
	{
		public string tenureType { get; set; }
		public object yearsRemainingOnLease { get; set; }
	}

	public class Image
	{
		public string url { get; set; }
		public string caption { get; set; }
		public Resizedimageurls resizedImageUrls { get; set; }
	}

	public class Resizedimageurls
	{
		public string size135x100 { get; set; }
		public string size476x317 { get; set; }
		public string size656x437 { get; set; }
	}

	public class Virtualtour
	{
		public string url { get; set; }
		public string caption { get; set; }
		public string videoId { get; set; }
		public string provider { get; set; }
	}

	public class Industryaffiliation
	{
		public string name { get; set; }
		public string imagePath { get; set; }
	}

	public class Room
	{
		public string name { get; set; }
		public string description { get; set; }
		public object width { get; set; }
		public object length { get; set; }
		public object unit { get; set; }
		public string dimension { get; set; }
	}

	public class Neareststation
	{
		public string name { get; set; }
		public string[] types { get; set; }
		public float distance { get; set; }
		public string unit { get; set; }
	}

	public class Brochure
	{
		public string url { get; set; }
		public string caption { get; set; }
	}

	public class Inforeelitem
	{
		public string title { get; set; }
		public string type { get; set; }
		public string primaryText { get; set; }
		public string secondaryText { get; set; }
	}

	public class Metadata
	{
		public string publicsiteUrl { get; set; }
		public string cookieDomain { get; set; }
		public string currencyCode { get; set; }
		public string emailAgentUrl { get; set; }
		public string facebookShareUrl { get; set; }
		public string twitterShareUrl { get; set; }
		public string emailShareUrl { get; set; }
		public string copyLinkUrl { get; set; }
		public string whatsAppShareUrl { get; set; }
		public string myRightmoveUrl { get; set; }
		public string mediaServerUrl { get; set; }
		public long serverTimestamp { get; set; }
		public string deviceType { get; set; }
		public string deviceOS { get; set; }
		public Mvtinfo[] mvtInfo { get; set; }
		public Featureswitches featureSwitches { get; set; }
		public string sentryUrl { get; set; }
		public Adunitpath adUnitPath { get; set; }
		public Backlink backLink { get; set; }
		public bool shouldTrackGTMSuccessTracker { get; set; }
		public Emailpreferences emailPreferences { get; set; }
		public string staticAssetsPath { get; set; }
		public string correlationId { get; set; }
		public string locationSearchUrl { get; set; }
		public string compareTheMarketExperimentId { get; set; }
		public string stampDutyCalculatorExperimentId { get; set; }
		public bool recaptchaEnabled { get; set; }
		public string recaptchaKey { get; set; }
	}

	public class Featureswitches
	{
		public bool GoogleTagManagerSwitchGTM_THIRD_PARTY_TAG_SWITCH { get; set; }
		public bool GoogleTagManagerSwitchGOOGLE_TAG_MANAGER_SWITCH { get; set; }
		public bool OnlineViewingsSwitchesONLINE_VIEWING_PAGE_ENABLED { get; set; }
		public bool PropertyDetailsWebUSE_RM_ASSISTANT { get; set; }
		public bool MRMSwitchesUSE_ASSISTANT_REGISTRATION { get; set; }
		public bool PropertyDetailsWebSTREETVIEW_BUTTON_ENABLED { get; set; }
		public bool PropertyDetailsWebUSE_TRAY_MORTGAGE { get; set; }
		public bool PropertyDetailsWebUSE_PREFER_EMAIL_CLIENT_ENABLED { get; set; }
		public bool PropertyDetailsWebGOOGLE_OPTIMISE_ENABLED { get; set; }
		public bool PropertyDetailsWebPARTIAL_CONTENT_ENABLED { get; set; }
		public bool PropertyDetailsWebAPP_DOWNLOAD_BANNER_ENABLED { get; set; }
		public bool PropertyDetailsWebWELCOME_BANNER_ENABLED { get; set; }
		public bool PropertyDetailsWebMARKET_INFO_LIMIT_THUMBNAILS { get; set; }
		public bool PropertyDetailsWebLOG_INVALID_COOKIE_ENABLED { get; set; }
		public bool PropertyDetailsWebADDRESS_PICKER_ENABLED { get; set; }
		public bool PropertyDetailsWebDEVELOPMENT_INFO_ENABLED { get; set; }
		public bool PropertyDetailsWebCAROUSEL_GALLERY_PAGE_ENABLED { get; set; }
		public bool PropertyDetailsWebI_AM_SOLD_LISTING_ENABLED { get; set; }
		public bool PropertyDetailsWebHIDE_HISTORICAL_RES_LET_CTA_ENABLED { get; set; }
		public bool PropertyDetailsWebSTAMP_DUTY_CALCULATOR_ENABLED { get; set; }
		public bool PropertyDetailsWebAFFORDABILITY_WIDGET_ENABLED { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesLABS_ROLLOUT_TURNED_ON { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesLABS_OVERSEAS { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesLABS_LETTINGS_RES { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesLABS_SALES_RES { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesLABS_LETTINGS_COM { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesLABS_SALES_COM { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesLABS_NEW_HOMES { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesLABS_STUDENT { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesROLLOUT_TURNED_ON { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesOVERSEAS { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesLETTINGS_RES { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesSALES_RES { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesLETTINGS_COM { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesSALES_COM { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesNEW_HOMES { get; set; }
		public bool NewPropertyDetailsPageRolloutSwitchesSTUDENT { get; set; }
		public bool CookieBarSwitchesBLOCK_UNKNOWN_COOKIES { get; set; }
		public bool CookieBarSwitchesENABLE_ONE_TRUST_COOKIE_SCRIPT { get; set; }
	}

	public class Adunitpath
	{
		public string mpu1 { get; set; }
		public string trackingPixel { get; set; }
	}

	public class Backlink
	{
		public string url { get; set; }
		public string text { get; set; }
		public int operation { get; set; }
	}

	public class Emailpreferences
	{
		public string preferencesUrl { get; set; }
		public bool showModal { get; set; }
		public string source { get; set; }
	}

	public class Mvtinfo
	{
		public string label { get; set; }
		public string state { get; set; }
		public bool shouldLog { get; set; }
	}

	public class Analyticsinfo
	{
		public Analyticsbranch analyticsBranch { get; set; }
		public Analyticsproperty analyticsProperty { get; set; }
	}

	public class Analyticsbranch
	{
		public string agentType { get; set; }
		public int branchId { get; set; }
		public string branchName { get; set; }
		public object branchPostcode { get; set; }
		public string brandName { get; set; }
		public string companyName { get; set; }
		public string companyType { get; set; }
		public string displayAddress { get; set; }
		public string pageType { get; set; }
	}

	public class Analyticsproperty
	{
		public string added { get; set; }
		public bool auctionOnly { get; set; }
		public int beds { get; set; }
		public bool businessForSale { get; set; }
		public string country { get; set; }
		public string currency { get; set; }
		public int floorplanCount { get; set; }
		public string furnishedType { get; set; }
		public bool hasOnlineViewing { get; set; }
		public int imageCount { get; set; }
		public float latitude { get; set; }
		public float longitude { get; set; }
		public bool letAgreed { get; set; }
		public string lettingType { get; set; }
		public object maxSizeAc { get; set; }
		public object maxSizeFt { get; set; }
		public object minSizeAc { get; set; }
		public object minSizeFt { get; set; }
		public string ownership { get; set; }
		public string postcode { get; set; }
		public string preOwned { get; set; }
		public int price { get; set; }
		public string priceQualifier { get; set; }
		public int propertyId { get; set; }
		public string propertySubType { get; set; }
		public string propertyType { get; set; }
		public bool retirement { get; set; }
		public object selectedCurrency { get; set; }
		public object selectedPrice { get; set; }
		public bool soldSTC { get; set; }
		public string videoProvider { get; set; }
		public string viewType { get; set; }
		public string customUri { get; set; }
	}
}
