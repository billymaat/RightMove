using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io.Network;
using Newtonsoft.Json;
using RightMove.DataTypes;
using RightMove.JsonObjects.NearbySoldPrices;
using RightMove.Services.Caching;

namespace RightMove.Services
{
	public class NearbySoldPricesService
	{
		private readonly HttpClient _httpClient;
		private readonly NearbySoldPropertiesCache _cache;

		public NearbySoldPricesService(HttpClient httpClient, NearbySoldPropertiesCache cache)
		{
			_httpClient = httpClient;
			_cache = cache;
		}

		public async Task<List<NearbySoldProperty>> GetNearbySoldPrices(string nearbyUrl, CancellationToken cancellationToken = default)
		{
			if (_cache.TryGetValue(nearbyUrl, out var rm))
			{
				return rm;
			}

			string url = $"https://www.rightmove.co.uk{nearbyUrl}";
			IDocument document = await GetDocumentAsync(url, cancellationToken).ConfigureAwait(false);

			if (document is null)
			{
				return null;
			}

			if (cancellationToken.IsCancellationRequested)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}

			var nearbyProperties = ParseNearbyPropertiesPage(document);
			_cache.CreateEntry(nearbyUrl, nearbyProperties);

			return nearbyProperties;
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

		private List<NearbySoldProperty> ParseNearbyPropertiesPage(IDocument document)
		{
			var json = GetJson(document);

			if (json is null)
			{
				return null;
			}

			if (json.searchResult.properties == null)
			{
				return null;
			}

			var result = json.searchResult.properties.Select(o =>
			{
				DateTime? dateSold;
				if (DateTime.TryParse(o.latestTransaction?.dateSold, out var d))
				{
					dateSold = d;
				}
				else
				{
					dateSold = null;
				}

				double? price = null;
				var priceString = o.latestTransaction?.displayPrice;
				if (!string.IsNullOrEmpty(priceString))
				{
					// Remove £ symbol and commas, then parse
					var cleanPrice = priceString.Replace("£", "").Replace(",", "").Trim();
					if (double.TryParse(cleanPrice, out var p))
					{
						price = p;
					}
				}
				
				return new NearbySoldProperty()
				{
					Address = o.address,
					Bathrooms = o.bathrooms ?? -1,
					Bedrooms = o.bedrooms ?? 1,
					PropertyType = o.propertyType,
					Price = price,
					DateSold = dateSold,
					Url = o.detailUrl
				};
			}).ToList();

			return result;
		}

		private static NearbySoldPricesResponse GetJson(IDocument document)
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
			text = text.Trim();

			var startIndex = text.IndexOf(start);
			if (startIndex < 0)
			{
				return null;
			}

			var jsonText = text.Substring(startIndex + start.Length, text.Length - 1 - startIndex - start.Length).Trim();

			var settings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore
			};

			var json = JsonConvert.DeserializeObject<NearbySoldPricesResponse>(jsonText, settings);
			return json;
		}
	}
}
