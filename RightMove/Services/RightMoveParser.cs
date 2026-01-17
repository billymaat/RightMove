using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RightMove.ApiResponse;
using RightMove.DataTypes;

namespace RightMove.Services
{
	public class RightMoveParser
	{
		public const int PriceNotSet = -1;

		private readonly ILogger<RightMoveParser> _logger;

		/// <summary>
		/// Initializes a new instance <see cref="RightMoveParser"/> class
		/// </summary>
		public RightMoveParser(ILogger<RightMoveParser> logger)
		{
			_logger = logger;
		}

		private string CreateUrl(SearchParams searchParams, int count)
		{
			int index = count * 24;
			return $"https://www.rightmove.co.uk/api/property-search/listing/search?{searchParams.EncodeOptions()}&index={index}&channel=BUY&transactionType=BUY";
		}

		/// <summary>
		/// Perform a search
		/// </summary>
		/// <returns>true if successful, false otherwise</returns>
		public async Task<RightMoveSearchItemCollection> SearchAsync(SearchParams searchParams)
		{
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
			httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36");

			var results = new List<RightMoveProperty>();
			var options = new JsonSerializerOptions
			{
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
			};

			for (int i = 0; i < 100; i++)
			{
				var url = CreateUrl(searchParams, i);
				var result = await httpClient.GetAsync(url).ConfigureAwait(false);

				if (result.IsSuccessStatusCode)
				{
					PropertySearchApiResponse propertyResponse = null;
					try
					{
						propertyResponse = await result.Content.ReadFromJsonAsync<PropertySearchApiResponse>(options).ConfigureAwait(false);
					}
					catch
					{
						// if we fail at all, just break out
						// TODO: logging, warning to user
						break;
					}

					if (propertyResponse.properties.Length == 0)
					{
						break;
					}

					var rightMoveProperties = propertyResponse.properties.Select(o => new RightMoveProperty()
					{
						RightMoveId = o.id,
						HouseInfo = o.propertyTypeFullDescription,
						Address = o.displayAddress,
						Desc = o.propertyTypeFullDescription,
						DateAdded = o.firstVisibleDate,
						DateUpdated = o.updateDate,
						Link = $"/properties/{o.id}",
						Agent = o.formattedBranchName?.Trim().StartsWith("by ") ?? false ? o.formattedBranchName.Trim().Substring(3) : o.formattedBranchName?.Trim(),
						ImageUrl = o.images.Select(img => $"https://media.rightmove.co.uk:443/dir/{img.url}").ToArray(),
						Price = o.price.amount,
					});
					results.AddRange(rightMoveProperties);
				}
			}

			return new RightMoveSearchItemCollection(results.ToList());
		}
	}
}
