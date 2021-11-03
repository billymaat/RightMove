using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Newtonsoft.Json;
using RightMove.DataTypes;
using RightMove.Factory;
using RightMove.JsonObjects;

namespace RightMove.Services
{
	public class PropertyPageParser : IPropertyPageParser
	{
		private IHttpService _httpService;
		private RightMovePropertyFactory _propertyFactory;
		
		public PropertyPageParser(IHttpService httpService, 
			RightMovePropertyFactory propertyFactory)
		{
			_propertyFactory = propertyFactory;
			_httpService = httpService;
		}

		public int PropertyId
		{
			get;
			set;
		}
		
		public RightMoveProperty RightMoveProperty
		{
			get;
			private set;
		}
		
		public RightMovePropertyPage Page
		{
			get;
			private set;
		}
		
		public Rootobject Json
		{
			get;
			private set;
		}

		public async Task<bool> ParseRightMovePropertyPageAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return await ParseRightMovePropertyPageAsync(PropertyId, cancellationToken);
		}

		public async Task<bool> ParseRightMovePropertyPageAsync(int propertyId, CancellationToken cancellationToken = default(CancellationToken))
		{
			string url = RightMoveUrls.GetPropertyUrl(propertyId);
			IDocument document = await _httpService.GetDocument(url, cancellationToken);

			if (document is null)
			{
				return false;
			}

			if (cancellationToken.IsCancellationRequested)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}
			
			ParseRightMovePropertyPage(document);
			return true;
		}

		private void ParseRightMovePropertyPage(IDocument document)
		{
			Json = GetJson(document);
			

			if (Json is null)
			{
				return;
			}

			RightMoveProperty property = _propertyFactory.Create();

			property.Address = $"{Json.propertyData.address.displayAddress}, {Json.propertyData.address.ukCountry}";
			property.Desc = Json.propertyData.text.description;
			property.Agent = Json.propertyData.customer.branchDisplayName;
			

			property.Price = RightMoveParserHelper.ParsePrice(Json.propertyData.prices.primaryPrice);
			property.DateAdded = RightMoveParserHelper.ParseDate(Json.propertyData.listingHistory.listingUpdateReason);
			property.ImageUrl = Json.propertyData.images.Select(o => o.url).ToArray();

			RightMoveProperty = property;
		}
		
		private static Rootobject GetJson(IDocument document)
		{
			var script = document.All.FirstOrDefault(o => o.LocalName.Equals("script") &&
			                                              o.Text().Trim().StartsWith("window.PAGE_MODEL"));

			if (string.IsNullOrEmpty(script?.Text()))
			{
				return null;
			}

			string start = "window.PAGE_MODEL = ";
			var indx = script?.Text().IndexOf(start);
			if (indx <= 0)
			{
				return null;
			}

			var jsonText = script.Text().Trim().Substring(start.Length);

			var settings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore
			};
			
			var json = JsonConvert.DeserializeObject<Rootobject>(jsonText, settings);
			return json;
		}
	}
}
