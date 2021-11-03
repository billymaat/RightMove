using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RightMove.Db.Repositories;
using RightMove.Db.Services;
using RightMove.Extensions;
using RightMove.Factory;
using RightMoveApp.Services;
using RightMoveApp.ViewModel;

namespace RightMoveApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly IHost host;
		public static IServiceProvider ServiceProvider { get; private set; }

		public static class WindowKeys
		{
			public const string MainWindow = nameof(MainWindow);
			public const string ImageWindow = nameof(ImageWindow);
		}

		public App()
		{
			host = Host.CreateDefaultBuilder()
				.ConfigureServices((context, services) =>
				{
					ConfigureServices(context.Configuration, services);
					services.Register();

					// register db writer
					services.AddTransient<IRightMovePropertyRepository, RightMovePropertyRepository>();
				})
				.Build();

			ServiceProvider = host.Services;
		}

		private void ConfigureServices(IConfiguration configuration,
			IServiceCollection services)
		{
			services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
			services.AddScoped<ISampleService, SampleService>();
			services.AddScoped<IDatabaseService, DatabaseService>();

			services.AddScoped<NavigationService>(serviceProvider =>
			{
				var navigationService = new NavigationService(serviceProvider);
				navigationService.Configure(WindowKeys.MainWindow, typeof(MainWindow));
				navigationService.Configure(WindowKeys.ImageWindow, typeof(ImageWindow));
				return navigationService;
			});

			// register RightMoveLibrary
			services.RegisterNew();

			// ...
			// Register all ViewModels
			services.AddSingleton<MainViewModel>();
			services.AddTransient<ImageViewModel>();

			// Register all the windows of the application
			services.AddSingleton<MainWindow>();
			services.AddTransient<ImageWindow>();
		}

		protected override async void OnStartup(StartupEventArgs e)
		{
			await host.StartAsync();
			var navigationService = ServiceProvider.GetRequiredService<NavigationService>();
			await navigationService.ShowAsync(WindowKeys.MainWindow);

			base.OnStartup(e);
		}

		protected override async void OnExit(ExitEventArgs e)
		{
			using (host)
			{
				await host.StopAsync(TimeSpan.FromSeconds(5));
			}

			base.OnExit(e);
		}
	}
}
