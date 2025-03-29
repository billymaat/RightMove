using System;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RightMove.Desktop.Model;
using RightMove.Desktop.Services;
using RightMove.Desktop.View;
using RightMove.Desktop.View.Main;
using RightMove.Desktop.ViewModel;
using RightMove.Extensions;
using Serilog;
using ServiceCollectionUtilities;

namespace RightMove.Desktop
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
				.UseSerilog()
				.ConfigureServices((context, services) =>
				{
					ConfigureServices(context.Configuration, services);
					// register db writer
					//services.AddTransient<IRightMovePropertyRepository, RightMovePropertyRepository>()
					//	.AddTransient<IDbConfiguration, DbConfiguration>(o => new DbConfiguration("RightMoveDB.db"));

				})
				.Build();

			ServiceProvider = host.Services;
		}

		private void ConfigureServices(IConfiguration configuration,
			IServiceCollection services)
		{			
			services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
			services.AddScoped<ISampleService, SampleService>();
			//services.AddScoped<IDatabaseService, DatabaseService>();

			services.AddScoped<NavigationService>(serviceProvider =>
			{
				var navigationService = new NavigationService(serviceProvider);
				navigationService.Configure(WindowKeys.MainWindow, typeof(MainWindow));
				navigationService.Configure(WindowKeys.ImageWindow, typeof(ImageWindow));
				return navigationService;
			});

			// register RightMoveLibrary
			services.RegisterRightMoveLibrary();
			RegisterView(services);
            RegisterServices(services);
            services.AddSingleton<IMessenger, WeakReferenceMessenger>();
        }

		private void RegisterView(IServiceCollection services)
        {
			services.AddSingleton<RightMoveModel>();

			// ...
			// Register all ViewModels
			services.AddSingleton<MainViewModel>()
                .AddSingleton<PropertyInfoViewModel>();

			// Register all the windows of the application
			services.AddSingleton<MainWindow>();
		}

        private void RegisterServices(IServiceCollection services)
        {
	        services.AddTransient<RightMoveService>()
		        .AddTransient<RightMoveSearchHistoryWriter>()
		        .AddTransient<RightMoveSearchHistoryReader>()
		        .AddTransient<SearchHistoryService>();

			services.AddFactory<RightMoveModel>();
        }

		protected override async void OnStartup(StartupEventArgs e)
        {
            CreateLogger();

            Log.Logger.Information("Starting up");
            await host.StartAsync();
            var navigationService = ServiceProvider.GetRequiredService<NavigationService>();
            await navigationService.ShowAsync(WindowKeys.MainWindow);

            base.OnStartup(e);
        }

        private static void CreateLogger()
        {
            // register logger
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                        .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
        }

        protected override async void OnExit(ExitEventArgs e)
		{
			using (host)
			{
				await host.StopAsync(TimeSpan.FromSeconds(5));
			}

			// Flush all Serilog sinks before the app closes
			Log.CloseAndFlush();

			base.OnExit(e);
		}
	}
}
