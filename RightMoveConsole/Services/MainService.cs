using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RightMove.DataTypes;
using RightMove.Db.Services;
using RightMove.Factory;

namespace RightMoveConsole.Services
{
	public class MainService : IHostedService
	{
		private int? _exitCode;

		private readonly RightMoveParserServiceFactory _rightMoveParserServiceFactory;
		private readonly IDatabaseService<RightMove.Db.Entities.RightMoveProperty> _db;
		private readonly IHostApplicationLifetime _appLifetime;
		private readonly ILogger _logger;
		private readonly IDisplayService _display;
		private readonly ISearchLocationsReader _searchLocationsReader;

		public MainService(IHostApplicationLifetime appLifetime,
			ILogger logger,
			RightMoveParserServiceFactory rightMoveParseServiceFactory,
			IDatabaseService<RightMove.Db.Entities.RightMoveProperty> db,
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
		private SearchParams GetSearchParams()
		{
			SearchParams searchParams = new SearchParams()
			{
				RegionLocation = "Ashton-Under-Lyne, Greater Manchester",
				Sort = SortType.HighestPrice,
				MinBedrooms = 2,
				MaxBedrooms = 3,
				MinPrice = 100000,
				MaxPrice = 500000,
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
		private async Task PerformSearch(SearchParams searchParams, bool updateDb = true)
		{
			_display.WriteLine("Search Parameters:");
			_display.WriteLine(searchParams.ToString());

			var rightMoveService = _rightMoveParserServiceFactory.CreateInstance(searchParams);
			Task<bool> res = rightMoveService.SearchAsync();

			await res;

			if (res.Result)
			{
				_display.WriteLine($"Results count: {rightMoveService.Results.Count}");

				if (updateDb)
				{
					var table = new string(searchParams.RegionLocation
						.Where(x => char.IsLetterOrDigit(x)).ToArray());
					(int newPropertiesCount, int updatedPropertiesCount) databaseUpdate = _db.AddToDatabase(rightMoveService.Results, table);

					_display.WriteLine($"New properties: {databaseUpdate.newPropertiesCount}");
					_display.WriteLine($"Updated properties: {databaseUpdate.updatedPropertiesCount}");
				}
			}

			_display.WriteLine();
		}

		private void WriteTimeAndDbFileLocation()
		{
			// write details
			Console.WriteLine($"Current time: {DateTime.Now}");
			Console.WriteLine($"DbFile: {_db.DbConfiguration.DbFile}");
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
					var searchParams = GetSearchParams();
					searchParams.RegionLocation = searchLocation;
					await PerformSearch(searchParams);
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
						WriteTimeAndDbFileLocation();
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

		//public Task StartAsync(CancellationToken cancellationToken)
		//{
		//	_logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

		//	_appLifetime.ApplicationStarted.Register(() =>
		//	{
		//		Task.Run(async () =>
		//		{
		//			try
		//			{
		//				_logger.LogInformation("Starting application");
		//				WriteTimeAndDbFileLocation();
		//				await PerformSearch();
		//				_exitCode = 0;
		//			}
		//			catch (Exception ex)
		//			{
		//				_logger.LogError(ex, "Unhandled exception!");
		//			}
		//			finally
		//			{
		//				// Stop the application once the work is done
		//				_appLifetime.StopApplication();
		//			}
		//		});
		//	});

		//	return Task.CompletedTask;
		//}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Exiting with return code: {_exitCode}");

			// Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
			Environment.ExitCode = _exitCode.GetValueOrDefault(-1);
			return Task.CompletedTask;
		}
	}
}
