using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace RightMoveConsole
{
	class Program
	{
		static async Task Main(string[] args)
		{
			await CreateHostBuilder(args).RunConsoleAsync();
		}

		static IHostBuilder CreateHostBuilder(string[] args)
		{
			var logger = new LoggerConfiguration()
				//.WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
				.WriteTo.Console()
				.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
				.CreateLogger();

			Log.Logger = logger;

			var host = Host.CreateDefaultBuilder(args)
				.ConfigureServices((_, services) =>
				{
					//using var loggerFactory = LoggerFactory.Create(builder =>
					//{
					//	builder
					//		.AddFilter("Microsoft", LogLevel.Warning)
					//		.AddFilter("System", LogLevel.Warning)
					//		.AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
					//		.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None)
					//		.AddConsole();
					//});
					//ILogger logger = loggerFactory.CreateLogger<Program>();
					//var logger = new LoggerConfiguration()
					//	.WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
					//	.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
					//	.CreateLogger();

					//logger.Warning("GDSJHFDSKFHDKSF");
					//Log.Logger = logger;
					//logger.LogInformation("Example log message");
					//Log.Logger = new LoggerConfiguration()
					//	.MinimumLevel.Verbose()
					//	.WriteTo.Console
					var builder = new ConfigurationBuilder()
						.SetBasePath(Directory.GetCurrentDirectory())
						.AddJsonFile("appsettings.json", optional: false)
						.AddEnvironmentVariables();

					IConfiguration config = builder.Build();
					services.AddLogging(x => x.AddSerilog());

					//services.AddSingleton<IDatabaseWritingService>(x => null);
					services.AddTransient<IDatabaseWritingService, DatabaseWritingService>();
					services.AddSingleton<IConfiguration>(x => config);
					//services.AddScoped<ILogger>(x => logger);
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

					var connectionString = !string.IsNullOrEmpty(envVar)
						? envVar
						: config.GetSection("ConnectionStrings:PostGre").Value;

					services.AddDbContext<RightMoveContext>(
						options => options.UseNpgsql(connectionString));
				});

			host.ConfigureLogging(logger =>
			{
				logger.ClearProviders();
				logger.AddSerilog();
			});

			return host;
		}
			
	}
}
