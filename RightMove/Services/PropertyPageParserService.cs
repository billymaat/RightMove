using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io.Network;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RightMove.DataTypes;
using RightMove.Helpers;
using RightMove.JsonObjects;
using RightMove.Services.Caching;

namespace RightMove.Services
{
	public class PropertyPageParserService
	{
		private readonly PropertyPageCache _cache;
		private readonly HttpClient _httpClient;

		public PropertyPageParserService(HttpClient httpClient, PropertyPageCache cache)
		{
			_httpClient = httpClient;
			_cache = cache;
		}
		
		public async Task<RightMoveProperty> ParseRightMovePropertyPageAsync(int propertyId, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (_cache.TryGetValue(propertyId, out var rm))
			{
				return rm;
			}
			string url = RightMoveUrls.GetPropertyUrl(propertyId);
			IDocument document = await GetDocumentAsync(url, cancellationToken).ConfigureAwait(false);

			if (document is null)
			{
				return null;
			}

			if (cancellationToken.IsCancellationRequested)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}

			var property = ParseRightMovePropertyPage(document);
			_cache.CreateEntry(propertyId, property);
			return property;
		}

		/// <summary>
		/// Get a Document from a url
		/// </summary>
		/// <param name="url">the url</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Returns the <see cref="IDocument"/></returns>
		private async Task<IDocument> GetDocumentAsync(string url, CancellationToken cancellationToken = default(CancellationToken))
		{
			var requester = new HttpClientRequester(_httpClient);

			// var config = Configuration.Default.WithDefaultLoader().WithDefaultCookies();
			var config = Configuration.Default.WithRequester(requester).WithDefaultLoader().WithDefaultCookies();
			var context = BrowsingContext.New(config);

			IDocument document = null;

			try
			{
				document = await context.OpenAsync(url, cancellationToken).ConfigureAwait(false);
			}
			catch (Exception)
			{
				document = null;
			}

			return document;
		}

		private RightMoveProperty ParseRightMovePropertyPage(IDocument document)
		{
			var json = GetJson(document);

			if (json is null)
			{
				return null;
			}

			RightMoveProperty property = new RightMoveProperty
			{
				Address = $"{json.propertyData.address.displayAddress}, {json.propertyData.address.ukCountry}"
			};

			property.Agent = json.propertyData.customer.branchDisplayName;
			property.Price = RightMoveParserHelper.ParsePrice(json.propertyData.prices.primaryPrice);
			property.DateAdded = RightMoveParserHelper.ParseDateAdded(json.propertyData.listingHistory.listingUpdateReason);
			property.DateUpdated = RightMoveParserHelper.ParseDateReduced(json.propertyData.listingHistory.listingUpdateReason);
			property.ImageUrl = json.propertyData.images.Select(o => o.url).ToArray();
			property.NearbySoldPricesUrl = json.propertyData.propertyUrls?.nearbySoldPropertiesUrl;

			var desc = json.propertyData.text.description;
			if (!string.IsNullOrEmpty(desc))
			{
				var htmklDoc = new HtmlDocument();
				htmklDoc.LoadHtml(desc);
				property.Desc = htmklDoc.DocumentNode.InnerText;
			}



			return property;
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
			string end = "window.adInfo";

			var text = script?.Text();

			var startIndex = text.IndexOf(start);
			if (startIndex <= 0)
			{
				return null;
			}

			var endIndex = text.IndexOf(end);
			if (endIndex <= startIndex)
			{
				return null;
			}

			var jsonText = text.Substring(startIndex + start.Length, endIndex - startIndex - start.Length).Trim();

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
