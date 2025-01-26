using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RightMove.DataTypes;

namespace RightMoveConsole.Services
{
	public class MainService : IHostedService
	{
		private int? _exitCode;

		private readonly IHostApplicationLifetime _appLifetime;
		private readonly ILogger<MainService> _logger;
		private readonly ISearchService _searchService;
		private readonly ISearchLocationsReader _searchLocationsReader;
		private readonly IServiceProvider _services;

		public MainService(IHostApplicationLifetime appLifetime,
			ILogger<MainService> logger,
			ISearchService searchService,
			ISearchLocationsReader searchLocationsReader,
			IServiceProvider services)
		{
			_appLifetime = appLifetime;
			_logger = logger;
			_searchService = searchService;
			_searchLocationsReader = searchLocationsReader;
			_services = services;
		}

		private IDatabaseWritingService CreateDatabaseWritingService()
		{
			using (var scope = _services.CreateScope())
			{
				IServiceProvider serviceProvider = scope.ServiceProvider;
				var service = serviceProvider.GetRequiredService<IDatabaseWritingService>();
				return service;
			}
		}

		/// <summary>
		/// Get search params
		/// </summary>
		/// <returns>the search params</returns>
		private SearchParams GetSearchParams(string regionLocation)
		{
			SearchParams searchParams = new SearchParams()
			{
				RegionLocation = regionLocation,
				Sort = SortType.HighestPrice,
				MinBedrooms = 1,
				MaxBedrooms = 10,
				MinPrice = 0,
				MaxPrice = 20000000,
				PropertyType = PropertyTypeEnum.None
			};

			return searchParams;
		}

		private void DisplayTimeAndDbFileLocation()
		{
			// write details
			_logger.LogInformation($"Current time: {DateTime.Now}");
		}

		private async Task PerformSearch()
		{			
			// get the search locations
			var searchLocations = _searchLocationsReader.GetLocations();


				if (searchLocations != null)
				{
					foreach (var searchLocation in searchLocations)
					{
						// perform searches
						var searchParams = GetSearchParams(searchLocation);

						// show the the properties
						_logger.LogInformation("Search Parameters:");
						_logger.LogInformation(searchParams.ToString());

						var results = await _searchService.Search(searchParams);

						var table = new string(searchParams.RegionLocation
							.Where(x => char.IsLetterOrDigit(x)).ToArray());
						using (var scope = _services.CreateScope())
						{
							IServiceProvider serviceProvider = scope.ServiceProvider;

							var db = serviceProvider.GetRequiredService<IDatabaseWritingService>();
							var databaseUpdate = db.AddProperty(results, table);
							_logger.LogInformation($"Results count: {results.Count}");
							_logger.LogInformation($"New properties: {databaseUpdate.NewProperties}");
							_logger.LogInformation($"Updated properties: {databaseUpdate.UpdatedProperties}");
					}
				}
			}
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

			_appLifetime.ApplicationStarted.Register(() =>
			{
				Task.Run(async () =>
				{
					try
					{
						_logger.LogInformation("Starting application");
						DisplayTimeAndDbFileLocation();
						await PerformSearch();
						_exitCode = 0;
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "Unhandled exception!");
					}
					finally
					{
						// Stop the application once the work is done
						_appLifetime.StopApplication();
					}
				});
			});

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Exiting with return code: {_exitCode}");

			// Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
			Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
			return Task.CompletedTask;
		}
	}
}
