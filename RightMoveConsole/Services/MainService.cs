using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RightMove.DataTypes;
using RightMove.Factory;
using RightMoveConsole.Models;
using RightMoveConsole.Repositories;

namespace RightMoveConsole.Services
{
	public class MainService : IHostedService
	{
		private int? _exitCode;

		private readonly RightMoveParserServiceFactory _rightMoveParserServiceFactory;
		private readonly IRightMovePropertyRepository _db;
		private readonly IHostApplicationLifetime _appLifetime;
		private readonly ILogger _logger;
		private readonly IDisplayService _display;

		public MainService(IHostApplicationLifetime appLifetime,
			ILogger logger,
			RightMoveParserServiceFactory rightMoveParseServiceFactory,
			IRightMovePropertyRepository db,
			IDisplayService display)
		{
			_appLifetime = appLifetime;
			_logger = logger;
			_rightMoveParserServiceFactory = rightMoveParseServiceFactory;
			_db = db;
			_display = display;
		}

		private async Task DoLoopSearch()
		{
			// for (int minPrice = 0; minPrice <= 3000000; minPrice += 10000) {
			for (int j = 0; j < SearchParams.AllowedPrices.Count - 1; j++)
			{	
				SearchParams searchParams = new SearchParams()
				{
					RegionLocation = "Manchester, Greater Manchester",
					Sort = SortType.HighestPrice,
					MinBedrooms = 1,
					MaxBedrooms = 10,
					MinPrice = SearchParams.AllowedPrices[j],
					MaxPrice = SearchParams.AllowedPrices[j + 1]
				};

				_logger.LogDebug("Starting search");
				_logger.LogDebug("Search param:");
				_logger.LogDebug(searchParams.ToString());

				var rightMoveService = _rightMoveParserServiceFactory.CreateInstance(searchParams);
				Task<bool> res = rightMoveService.SearchAsync();

				await res;

				if (res.Result)
				{
					var dbProperties = _db.LoadProperties();
					List<RightMoveProperty> updatedProperties = new List<RightMoveProperty>();
					List<RightMoveProperty> newProperties = new List<RightMoveProperty>();

					_logger.LogDebug($"Results count: {rightMoveService.Results.Count}");
					for (int i = 0; i < rightMoveService.Results.Count; i++)
					{
						var property = rightMoveService.Results[i];
						var matchingProperty = dbProperties.FirstOrDefault(o => o.RightMoveId.Equals(property.RightMoveId));

						if (matchingProperty != null)
						{
							// if the price has changed, add the new price
							if (matchingProperty.Prices.Last() != property.Price)
							{
								_db.AddPriceToProperty(matchingProperty.Id, property.Price);
								updatedProperties.Add(property);
							}
						}
						else
						{
							// save a new record of the new property
							_db.SaveProperty(new RightMovePropertyModel(property));
							newProperties.Add(property);
						}
					}
				}
			}
		}

		private SearchParams GetPrestwichSearchParams()
		{
			SearchParams searchParams = GetSearchParams();
			searchParams.RegionLocation = "Prestwich, Manchester, Greater Manchester";

			return searchParams;
		}

		private SearchParams GetAshtonSearchParams()
		{
			SearchParams searchParams = GetSearchParams();
			searchParams.RegionLocation = "Ashton-Under-Lyne, Greater Manchester";
			return searchParams;
		}

		private SearchParams GetManchesterSearchParams()
		{
			SearchParams searchParams = GetSearchParams();
			searchParams.RegionLocation = "Manchester, Greater Manchester";

			return searchParams;
		}

		private SearchParams GetDroylsdenSearchParams()
		{
			SearchParams searchParams = GetSearchParams();
			searchParams.RegionLocation = "Droylsden, Greater Manchester";

			return searchParams;
		}

		private SearchParams GetAudenshawSearchParams()
		{
			SearchParams searchParams = GetSearchParams();
			searchParams.RegionLocation = "Audenshaw, Manchester, Greater Manchester";

			return searchParams;
		}

		private SearchParams GetDukinfieldSearchParams()
		{
			SearchParams searchParams = GetSearchParams();
			searchParams.RegionLocation = "Dukinfield, Cheshire";

			return searchParams;
		}

		private SearchParams GetStalybridgeSearchParams()
		{
			SearchParams searchParams = GetSearchParams();
			searchParams.RegionLocation = "Stalybridge, Greater Manchester";

			return searchParams;
		}

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
			};

			return searchParams;
		}

		private async Task DoSearch(SearchParams searchParams)
		{
			_display.WriteLine("Starting search");
			_display.WriteLine(searchParams.ToString());

			var rightMoveService = _rightMoveParserServiceFactory.CreateInstance(searchParams);
			Task<bool> res = rightMoveService.SearchAsync();

			await res;

			if (res.Result)
			{
				var dbProperties = _db.LoadProperties();
				List<RightMoveProperty> updatedProperties = new List<RightMoveProperty>();
				List<RightMoveProperty> newProperties = new List<RightMoveProperty>();
				
				_display.WriteLine($"Results count: {rightMoveService.Results.Count}");
				for (int i = 0; i < rightMoveService.Results.Count; i++)
				{
					var property = rightMoveService.Results[i];
					// _display.WriteLine($"{i}: {property}");
					var matchingProperty = dbProperties.FirstOrDefault(o => o.RightMoveId.Equals(property.RightMoveId));

					if (matchingProperty != null)
					{
						// if the price has changed, add the new price
						if (matchingProperty.Prices.Last() != property.Price)
						{
							_db.AddPriceToProperty(matchingProperty.Id, property.Price);
							updatedProperties.Add(property);
						}
					}
					else
					{
						// save a new record of the new property
						_db.SaveProperty(new RightMovePropertyModel(property));
						newProperties.Add(property);
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
						_logger.LogInformation("Hello World!");

						// perform searches
						await DoSearch(GetAshtonSearchParams());
						await DoSearch(GetManchesterSearchParams());
						await DoSearch(GetPrestwichSearchParams());
						await DoSearch(GetStalybridgeSearchParams());
						await DoSearch(GetDukinfieldSearchParams());
						await DoSearch(GetAudenshawSearchParams());
						await DoSearch(GetDroylsdenSearchParams());

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
