using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RightMove.DataTypes;
using RightMove.Db.Entities;
using RightMove.Db.Services;
using RightMove.Factory;
using RightMoveConsole.Models;

namespace RightMoveConsole.Services
{
	public class MainService : IHostedService
	{
		private int? _exitCode;

		private readonly RightMoveParserServiceFactory _rightMoveParserServiceFactory;
		private readonly IDatabaseService<RightMovePropertyEntity> _db;
		private readonly IHostApplicationLifetime _appLifetime;
		private readonly ILogger _logger;
		private readonly IDisplayService _display;
		private readonly ISearchLocationsReader _searchLocationsReader;

		public MainService(IHostApplicationLifetime appLifetime,
			ILogger logger,
			RightMoveParserServiceFactory rightMoveParseServiceFactory,
			IDatabaseService<RightMovePropertyEntity> db,
			IDisplayService display,
			ISearchLocationsReader searchLocationsReader)
		{
			_appLifetime = appLifetime;
			_logger = logger;
			_rightMoveParserServiceFactory = rightMoveParseServiceFactory;
			_db = db;
			_display = display;
			_searchLocationsReader = searchLocationsReader;
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

		/// <summary>
		/// Do the search and update the database
		/// </summary>
		/// <param name="searchParams">the search params</param>
		/// <param name="updateDb">true to update db, false otherwise</param>
		/// <returns></returns>
		private async Task<RightMoveSearchResult> PerformSearch(SearchParams searchParams, bool updateDb = true)
		{
			var rightMoveService = _rightMoveParserServiceFactory.CreateInstance(searchParams);
			bool res = await rightMoveService.SearchAsync();
			
			if (!res)
			{
				// failed
				return null;
			}

			var rightMoveSearchResult = new RightMoveSearchResult();
			rightMoveSearchResult.ResultsCount = rightMoveService.Results.Count;

			if (updateDb)
			{
				var table = new string(searchParams.RegionLocation
					.Where(x => char.IsLetterOrDigit(x)).ToArray());
				var databaseUpdate = _db.AddToDatabase(rightMoveService.Results, table);

				rightMoveSearchResult.NewPropertiesCount = databaseUpdate.NewProperties;
				rightMoveSearchResult.UpdatedPropertiesCount = databaseUpdate.UpdatedProperties;
			}

			return rightMoveSearchResult;
		}

		private void DisplayTimeAndDbFileLocation()
		{
			// write details
			_display.WriteLine($"Current time: {DateTime.Now}");
			_display.WriteLine($"DbFile: {_db.DbConfiguration.DbFile}");
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
					_display.WriteLine("Search Parameters:");
					_display.WriteLine(searchParams.ToString());

					var results = await PerformSearch(searchParams);
					_display.WriteLine($"Results count: {results.ResultsCount}");
					_display.WriteLine($"New properties: {results.NewPropertiesCount}");
					_display.WriteLine($"Updated properties: {results.UpdatedPropertiesCount}");
					_display.WriteLine();
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
