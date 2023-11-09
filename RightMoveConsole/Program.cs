﻿using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RightMove.Db;
using RightMove.Db.Entities;
using RightMove.Db.Extensions;
using RightMove.Db.Repositories;
using RightMove.Extensions;
using RightMove.Factory;
using RightMoveConsole.Services;

namespace RightMoveConsole
{
	class Program
	{
		static async Task Main(string[] args)
		{
			await CreateHostBuilder(args).RunConsoleAsync();
		}

		static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((_, services) =>
					{
						using var loggerFactory = LoggerFactory.Create(builder =>
						{
							builder
								.AddFilter("Microsoft", LogLevel.Warning)
								.AddFilter("System", LogLevel.Warning)
								.AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
								.AddConsole();
						});
						ILogger logger = loggerFactory.CreateLogger<Program>();
						logger.LogInformation("Example log message");

						services.AddScoped<ILogger>(x => logger);
						services.RegisterRightMoveLibrary();
						services.RegisterRightMoveDb();
						services
							.AddScoped<ISearchService, SearchService>()
							.AddScoped<IRightMoveParserFactory, RightMoveParserFactory>()
							.AddTransient<IRightMovePropertyRepository<RightMovePropertyEntity>, RightMovePropertyEFRepository>()
							.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILogger<MainService>>())
							.AddSingleton<ISearchLocationsReader>(new SearchLocationsReader(() => "searchlocations.txt"))
							.AddHostedService<MainService>();

						services.AddDbContext<RightMoveContext>();
					}
				);
	}
}
