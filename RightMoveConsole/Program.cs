﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RightMove.DataTypes;
using RightMove.Db.Repositories;
using RightMove.Extensions;
using RightMove.Factory;
using RightMove.Services;
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
						services.RegisterNew();
						services.AddScoped<IDisplayService, DisplayService>()
							.AddTransient<IRightMovePropertyRepository, RightMovePropertyRepository>()
							.AddSingleton<ILogger>(provider => provider.GetRequiredService<ILogger<MainService>>())
							.AddSingleton<ISearchLocationsReader>(new SearchLocationsReader(() => "searchlocations.txt"))
							.AddHostedService<MainService>();
					}
				);
	}
}
