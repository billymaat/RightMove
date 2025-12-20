using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using RightMove.Services;
using RightMove.Services.Caching;

namespace RightMove.Extensions
{
	public static class ServiceCollectionExtensions
	{
        /// <summary>
        /// Registers the RightMove library services with the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        public static void RegisterRightMoveLibrary(this IServiceCollection services)
        {
	        services.AddTransient<IHttpService, HttpService>()
		        .AddScoped<RightMoveRegionService>()
		        .AddTransient<RightMoveParser>();

	        services.AddHttpClient<PropertyPageParserService>(client =>
		        {
			        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
		        })
		        .AddPolicyHandler(GetRetryPolicy());

			services.AddHttpClient<NearbySoldPricesService>(client =>
				{
					client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");
				})
				.AddPolicyHandler(GetRetryPolicy());

			services.RegisterPropertyPageCaching();
        }

        private static void RegisterPropertyPageCaching(this IServiceCollection services)
        {
	        services.AddMemoryCache()
		        .AddTransient<PropertyPageCache>()
		        .AddTransient<NearbySoldPropertiesCache>();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
	        return HttpPolicyExtensions
		        .HandleTransientHttpError()
		        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
		        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
	}
}
