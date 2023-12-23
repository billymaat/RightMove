using System;
using Microsoft.Extensions.DependencyInjection;
using RightMove.DataTypes;
using RightMove.Factory;
using RightMove.Services;

namespace RightMove.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterRightMoveLibrary(this IServiceCollection services)
		{
			services.AddTransient<IHttpService, HttpService>()
				.AddScoped<RightMoveOutcodeService>()
				.AddScoped<RightMoveRegionService>()
				.AddScoped<RightMoveParserFactory>()
				.AddScoped<RightMovePropertyFactory>()
				.AddTransient<PropertyPageParser>()
				//.AddTransient<RightMoveParser>()
				.AddTransient<SearchPageParserServiceFactory>()
				.AddTransient<IActivator, ActivatorInjector>()
				.AddFactory<IPropertyPageParser, PropertyPageParser>();
		}

		public static void AddFactory<TService, TImplementation>(this IServiceCollection services)
			where TService : class
			where TImplementation : class, TService
		{
			services.AddTransient<TService, TImplementation>();
			services.AddSingleton<Func<TService>>(x => () => x.GetService<TService>());
			services.AddSingleton<IFactory<TService>, Factory<TService>>();
		}
	}
}
