using System;
using Microsoft.Extensions.DependencyInjection;
using RightMove.DataTypes;
using RightMove.Factory;
using RightMove.Services;

namespace RightMove.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void Register(this IServiceCollection services)
		{
			// services.AddTransient<RightMoveParserService>(x => Activator.CreateInstance<RightMoveParserService>(x, new SearchParams()));
			services.AddTransient<RightMovePropertyFactory>();
			services.AddTransient<IHttpService, HttpService>();
			services.AddTransient<RightMoveParserService>();
			services.AddTransient<PropertyPageParser>();
			services.AddTransient<SearchPageParserService>();
		}

		public static void RegisterNew(this IServiceCollection services)
		{
			services.AddTransient<IHttpService, HttpService>()
				.AddScoped<ILoggerService, LoggerService>()
				.AddScoped<RightMoveOutcodeService>()
				.AddScoped<RightMoveRegionService>()
				.AddScoped<RightMoveParserServiceFactory>()
				.AddScoped<RightMovePropertyFactory>()
				.AddTransient<PropertyPageParser>()
				.AddTransient<RightMoveParserService>()
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
