using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AngleSharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RightMove;
using RightMove.DataTypes;
using RightMove.Extensions;
using RightMove.Factory;
using RightMove.Services;
using RightMoveConsole.Models;
using RightMoveConsole.Repositories;
using RightMoveConsole.Services;

namespace RightMoveConsole
{
	class Program
	{
		static async Task Main(string[] args)
		{
			await CreateHostBuilder(args).RunConsoleAsync();

			//using IHost host = CreateHostBuilder(args).Build();

			//var mainService = host.Services.GetService<MainService>();
			//// await DoWork(host.Services);
			//// await mainService.DoWork();
			//// return host.RunAsync();
			//await DoWork(host.Services);
			//// await host.RunAsync();
			//await host.RunCon

			//using IHost host = CreateHostBuilder(args).Build();

			//// Console.WriteLine("Faking it");
			//// Task.Run(() => FakeWebAsync());
			//Console.WriteLine("Staring rightmove search");
			//var rightMoveParser = host.Services.GetRequiredService<RightMoveParser>();

			//var reg = RightMoveCodes.RegionDictionary;
			//SearchParams searchParams = new SearchParams()
			//{
			//	RegionLocation = "Manchester, Greater Manchester",
			//	// OutcodeLocation = "OL6",
			//	MinBedrooms = 0,
			//	MaxBedrooms = 5,
			//	MinPrice = 100000,
			//	MaxPrice = 500000,
			//};

			//// Url.Create(@"https://www.rightmove.co.uk/property-for-sale/find.html?locationIdentifier=REGION%5e904&maxBedrooms=5&minPrice=100000&maxPrice=500000&sortType=6");
			//rightMoveParser.SearchParams = searchParams;

			//Task t = Task.Run(() => rightMoveParser.SearchAsync());

			//t.ContinueWith((x) =>
			//{
			//	Console.WriteLine($"Task finished, count: {rightMoveParser.Results.Count}");
			//});

			////TestClass testClass = new TestClass();
			////Task t = Task.Run(() => testClass.Run());

			////t.ContinueWith((x) =>
			////{
			////	Console.WriteLine("Finished exceptions");
			////});

			//while (true)
			//{
			//	await Task.Delay(100);
			//	Console.Write(".");
			//}

			//Console.WriteLine("Finished");

			//await host.RunAsync();
		}

		private static async Task DoWork(IServiceProvider services)
		{
			//SearchParams searchParams = new SearchParams()
			//{
			//	RegionLocation = "Ashton-Under-Lyne, Greater Manchester",
			//	MinBedrooms = 0,
			//	MaxBedrooms = 5,
			//	MinPrice = 100000,
			//	MaxPrice = 10000000,
			//	Sort = SortType.HighestPrice,
			//	Radius = 0
			//};


			using IServiceScope serviceScope = services.CreateScope();
			IServiceProvider provider = serviceScope.ServiceProvider;

			var display = provider.GetRequiredService<IDisplayService>();
			
			SearchParams searchParams = new SearchParams()
			{
				RegionLocation = "Manchester, Greater Manchester",
				// OutcodeLocation = "OL6",
				MinBedrooms = 0,
				MaxBedrooms = 5,
				MinPrice = 0,
				// MaxPrice = 10000000,
				MaxPrice = 150000
			};


			var rightMoveService = provider.GetRequiredService<RightMoveParserServiceFactory>()
				.CreateInstance(searchParams);
			display.WriteLine("Starting search");
			display.WriteLine(searchParams.ToString());
			
			Task<bool> res = rightMoveService.SearchAsync();

			//while (!res.IsCompleted)
			//{
			//	await Task.Delay(100);
			//	Console.Write(".");
			//}
			await res;

			var db = services.GetRequiredService<IRightMovePropertyRepository>();

			if (res.Result)
			{
				display.WriteLine($"Results count: {rightMoveService.Results.Count}");
				for (int i = 0; i < rightMoveService.Results.Count; i++)
				{
					var	property = rightMoveService.Results[i];
					display.WriteLine($"{i}: {property}");
					db.SaveProperty(new RightMovePropertyModel(property));
				}
			}
		}

		static async Task FakeWebAsync()
		{
			Debug.WriteLine("Call AccessTheWebAsync");
			await Task.Delay(5000);
			Debug.WriteLine("Call AccessTheWebAsync done");
		}
		//static IHostBuilder CreateHostBuilder(string[] args) =>
		//	Host.CreateDefaultBuilder(args)
		//		.ConfigureServices((_, services) =>
		//			services.Register()
		//		);

		static IHostBuilder CreateHostBuilder(string[] args) =>
		//Host.CreateDefaultBuilder(args)
		//	.ConfigureServices((_, services) =>
		//		services.AddTransient<IHttpService, HttpService>()
		//			.AddScoped<ILoggerService, LoggerService>()
		//			.AddScoped<IDisplayService, DisplayService>()
		//			.AddScoped<RightMoveOutcodeService>()
		//			.AddScoped<RightMoveRegionService>()
		//			.AddScoped<RightMoveParserServiceFactory>()
		//			.AddTransient<RightMovePropertyFactory>()
		//			.AddTransient<PropertyPageParser>()
		//			.AddTransient<RightMoveParserService>()
		//			.AddTransient<SearchPageParserService>()
		//			.AddTransient<IRightMovePropertyRepository, RightMovePropertyRepository>()
		//			.AddTransient<SearchPageParserServiceFactory>()
		//			.AddSingleton<MainService>()
		//	);

		//Host.CreateDefaultBuilder(args)
		//	.ConfigureServices((_, services) =>
		//		services.AddTransient<IHttpService, HttpService>()
		//			.AddScoped<ILoggerService, LoggerService>()
		//			.AddScoped<IDisplayService, DisplayService>()
		//			.AddScoped<RightMoveOutcodeService>()
		//			.AddScoped<RightMoveRegionService>()
		//			.AddScoped<RightMoveParserServiceFactory>()
		//			.AddScoped<RightMovePropertyFactory>()
		//			.AddTransient<PropertyPageParser>()
		//			.AddTransient<RightMoveParserService>()
		//			// .AddTransient<SearchPageParserService>()
		//			.AddTransient<IRightMovePropertyRepository, RightMovePropertyRepository>()
		//			.AddTransient<SearchPageParserServiceFactory>()
		//			// .AddSingleton<MainService>()
		//			.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILogger<MainService>>())
		//			.AddHostedService<MainService>()
		//	);

		Host.CreateDefaultBuilder(args)
			.ConfigureServices((_, services) =>
				{
					services.RegisterNew();
					services.AddScoped<IDisplayService, DisplayService>()
						.AddTransient<IRightMovePropertyRepository, RightMovePropertyRepository>()
						.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILogger<MainService>>())
						.AddHostedService<MainService>();
				}
			);
	}
}
