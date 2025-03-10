using Microsoft.Extensions.DependencyInjection;
using RightMove.Factory;
using RightMove.Services;
using ServiceCollectionUtilities;

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
            .AddScoped<RightMoveParserFactory>()
            .AddScoped<RightMovePropertyFactory>()
            .AddTransient<PropertyPageParser>()
            .AddTransient<RightMoveParser>()
            .AddTransient<SearchPageParserServiceFactory>()
            .AddTransient<IActivator, ActivatorInjector>()
            .AddFactory<IPropertyPageParser, PropertyPageParser>();
        }
	}
}
