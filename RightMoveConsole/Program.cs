using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

						var builder = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json", optional: false)
							.AddEnvironmentVariables();

						IConfiguration config = builder.Build();

						//services.AddSingleton<IDatabaseWritingService>(x => null);
						services.AddSingleton<IDatabaseWritingService, DatabaseWritingService>();
						services.AddSingleton<IConfiguration>(x => config);
						services.AddScoped<ILogger>(x => logger);
						services.RegisterRightMoveLibrary();
						services.RegisterRightMoveDb();
						services
							.AddSingleton<ISearchService, SearchService>()
							.AddSingleton<IRightMoveParserFactory, RightMoveParserFactory>()
							.AddTransient<IRightMovePropertyRepository<RightMovePropertyEntity>, RightMovePropertyEFRepository>()
							.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILogger<MainService>>())
							.AddSingleton<ISearchLocationsReader>(new SearchLocationsReader(() => "searchlocations.txt"))
							.AddHostedService<MainService>();

						var envVar = Environment.GetEnvironmentVariable("ConnectionString");
						Console.WriteLine($"envVar: {envVar}");
						//services.AddDbContext<RightMoveContext>(options => options.UseSqlServer(config.GetSection("ConnectionStrings:Default").Value));

						var connectionString = !string.IsNullOrEmpty(envVar)
							? envVar
							: config.GetSection("ConnectionStrings:MariaDb").Value;
						services.AddDbContext<RightMoveContext>(
							options => options.UseMySql(connectionString,
							new MariaDbServerVersion(new Version(10, 3, 39))));
					}
				);
	}
}
