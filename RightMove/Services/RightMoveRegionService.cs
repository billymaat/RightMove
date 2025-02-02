using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using RightMove.ApiResponse;
using RightMove.DataTypes;

namespace RightMove
{
    public class RightMoveRegionService
    {
        private readonly string _apiEndpoint = $@"https://los.rightmove.co.uk/typeahead?query={{0}}&limit=10&exclude=STREET";

        public async Task<IEnumerable<RightMoveRegion>> SearchAsync(string search, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            // strip out any non alphanumeric characters
            var region = new string(search.Where(char.IsLetterOrDigit).ToArray());

            // Encode region for the url
            var encodedRegion = UrlEncoder.Default.Encode(region);

            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync(string.Format(_apiEndpoint, encodedRegion), cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                var regionApiResponse = await result.Content.ReadFromJsonAsync<RegionApiResponse>(cancellationToken);
                var ret = regionApiResponse.matches.Select(x => new RightMoveRegion
                {
                    Id = x.id,
                    DisplayName = x.displayName
                });
                return ret;
            }
            
            // Handle error response
            throw new HttpRequestException($"Request failed with status code {result.StatusCode}");
        }
    }
}
