using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Extensions.Options;
using RightMove.DataTypes;
using RightMove.Db.Models;
using RightMove.Db.Repositories;
using RightMove.Db.Services;
using RightMove.Factory;
using RightMove.Services;
using RightMoveApp.Helpers;
using RightMoveApp.Model;
using RightMoveApp.Services;
using RightMoveApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RightMoveApp.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		// Services
		private readonly NavigationService _navigationService;

		// The database service
		private readonly IDatabaseService _dbService;

		private int _selectedImageIndex;

		// Backing fields
		private string _info;
		private BitmapImage _displayedImage;
		private bool _loadingImage;
		private string _imageIndexView;

		// cancellation token
		private CancellationTokenSource _tokenSource = new CancellationTokenSource();

		// Time for selected item changed in data grid
		private System.Windows.Threading.DispatcherTimer _selectedItemChangedTimer;

		// The right move model
		private readonly RightMoveModel _rightMoveModel;
		private readonly AppSettings _settings;
		private readonly RightMoveParserServiceFactory _parserFactory;
		private readonly Func<IPropertyPageParser> _propertyParserFactory;

		public MainViewModel(IOptions<AppSettings> settings,
			RightMoveModel rightMoveModel,
			RightMoveParserServiceFactory parserFactory,
			NavigationService navigationService,
			IDatabaseService dbService,
			Func<IPropertyPageParser> propertyParserFactory)
		{
			_settings = settings.Value;
			_parserFactory = parserFactory;
			_navigationService = navigationService;
			_dbService = dbService;
			_propertyParserFactory = propertyParserFactory;

			InitializeCommands();
			InitializeTimers();

			_rightMoveModel = rightMoveModel;
			_rightMoveModel.PropertyChanged += RightMoveModel_PropertyChanged;
			IsSearching = false;
		}

		private void RightMoveModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(_rightMoveModel.RightMovePropertyItems))
			{
				RaisePropertyChanged(nameof(RightMoveList));
			}
			else if (e.PropertyName == nameof(_rightMoveModel.RightMovePropertyFullSelectedItem))
			{
				RaisePropertyChanged(nameof(RightMovePropertyFullSelectedItem));
			}
		}

		public bool IsImagesVisible
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the Info
		/// </summary>
		public string Info
		{
			get => _info;
			set => Set(ref _info, value);
		}

		/// <summary>
		/// Gets or sets the right move items
		/// </summary>
		public RightMoveSearchItemCollection RightMoveList
		{
			get => _rightMoveModel.RightMovePropertyItems;
		}

		private RightMoveProperty _rightMoveSelectedItem;
		/// <summary>
		/// Gets or sets the selected <see cref="RightMoveViewItem"/>
		/// </summary>
		public RightMoveProperty RightMoveSelectedItem
		{
			get => _rightMoveSelectedItem;
			set => Set(ref _rightMoveSelectedItem, value);
		}

		public RightMoveProperty RightMovePropertyFullSelectedItem
		{
			get => _rightMoveModel.RightMovePropertyFullSelectedItem;
		}

		public List<int> Prices
		{
			get
			{
				if (RightMoveSelectedItem is null)
				{
					return null;
				}

				var dbProperties = _dbService.LoadProperties();
				var matchingProperty = dbProperties.FirstOrDefault(o => o.RightMoveId.Equals(RightMoveSelectedItem.RightMoveId));

				if (matchingProperty is null)
				{
					return null;
				}

				var prices = matchingProperty.Prices;

				return prices;
			}
		}

		/// <summary>
		/// Gets the price history for right move property
		/// </summary>
		public string PriceHistory
		{
			get
			{
				string na = "N/A";

				if (RightMoveSelectedItem is null)
				{
					return na;
				}

				var dbProperties = _dbService.LoadProperties();
				var matchingProperty = dbProperties.FirstOrDefault(o => o.RightMoveId.Equals(RightMoveSelectedItem.RightMoveId));

				if (matchingProperty is null)
				{
					return na;
				}

				var dates = matchingProperty.DatesUpdated;
				var prices = matchingProperty.Prices;

				var combined = dates.Zip(prices, (d, p) => $"{DateTime.Parse(d).Date.ToString("dd/MM/yyyy")} : £{p}");
				var priceString = string.Join("\n", combined);
				return priceString;
			}
		}

		public bool LoadingImage
		{
			get => _loadingImage;
			set => Set(ref _loadingImage, value);
		}

		private SearchParams _searchParams;

		/// <summary>
		/// Gets or sets the <see cref="SearchParams"/>
		/// </summary>
		public SearchParams SearchParams
		{
			get => _searchParams;
			set
			{
				Set(ref _searchParams, value);
				SearchAsyncCommand.RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		/// Gets or sets the displayed image
		/// </summary>
		public BitmapImage DisplayedImage
		{
			get => _displayedImage;
			set => Set(ref _displayedImage, value);
		}

		private bool _isSearching;
		/// <summary>
		/// Gets or sets a value indicating whether searching is occurring
		/// </summary>
		public bool IsSearching 
		{ 
			get => _isSearching;
			set
			{
				Set(ref _isSearching, value);
				SearchAsyncCommand.RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		/// Gets a value indicating whether selected item has images available
		/// </summary>
		public bool HasImages => RightMovePropertyFullSelectedItem != null && RightMovePropertyFullSelectedItem.ImageUrl.Length > 0;

		public string ImageIndexView
		{
			get => _imageIndexView;
			set => Set(ref _imageIndexView, value);
		}

		private bool _hasSearchExecuted;

		public bool HasSearchedExecuted
		{
			get => _hasSearchExecuted;
			set => Set(ref _hasSearchExecuted, value);
		}

		#region Commands

		/// <summary>
		/// Gets or sets the search command
		/// </summary>
		public IAsyncCommand SearchAsyncCommand
		{
			get;
			set;
		}

		public ICommand SearchParamsUpdatedCommand
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the open link command
		/// </summary>
		public ICommand OpenLink
		{
			get;
			set;
		}

		public ICommand LoadImageWindow
		{
			get;
			set;
		}

		// Tried to get this AsyncCommand to work but it wouldn't
		public ICommand UpdateImages
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the NextImageCommand
		/// </summary>
		public IAsyncCommand NextImageCommand
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the PrevImageCommand
		/// </summary>
		public IAsyncCommand PrevImageCommand
		{
			get;
			set;
		}

		#endregion

		#region Command methods

		/// <summary>
		/// Show image window
		/// </summary>
		/// <param name="obj"></param>
		/// <returns>Task to load image window</returns>
		private Task ExecuteLoadImageWindowAsync(object obj)
		{
			return _navigationService.ShowDialogAsync(App.WindowKeys.ImageWindow, RightMoveSelectedItem.RightMoveId);
		}

		/// <summary>
		/// Can execute load image window
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		private bool CanExecuteLoadImageWindow(object arg)
		{
			return RightMoveSelectedItem != null;
		}

		#endregion

		private async Task UpdateFullSelectedItemAndImage(CancellationToken cancellationToken)
		{
			await UpdateRightMovePropertyFullSelectedItem(cancellationToken);
			await UpdateImage(cancellationToken);
			LoadingImage = false;
		}

		private async Task UpdateRightMovePropertyFullSelectedItem(CancellationToken cancellationToken)
		{
			_selectedImageIndex = 0;

			// update the right move full selected item
			await _rightMoveModel.GetFullRightMoveItem(RightMoveSelectedItem.RightMoveId, cancellationToken);
		}

		/// <summary>
		/// Update the displayed image
		/// </summary>
		/// <param name="cancellationToken">the cancellation token</param>
		/// <returns></returns>
		private async Task<BitmapImage> UpdateImage(CancellationToken cancellationToken)
		{
			LoadingImage = true;

			try
			{
				DisplayedImage = await _rightMoveModel.GetImage(_selectedImageIndex);
				// update the image view
				UpdateImageIndexView();

				PrevImageCommand.RaiseCanExecuteChanged();
				NextImageCommand.RaiseCanExecuteChanged();
				return DisplayedImage;
			}
			catch (OperationCanceledException e)
			{
				Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
				return null;
			}
			finally
			{
				LoadingImage = false;
			}
		}

		private void InitializeTimers()
		{
			_selectedItemChangedTimer = new System.Windows.Threading.DispatcherTimer();
			_selectedItemChangedTimer.Interval = TimeSpan.FromMilliseconds(500);
			_selectedItemChangedTimer.Tick += SelectedItemChanged_Elapsed;
		}

		/// <summary>
		/// Initialize a bunch of <see cref="ICommand"/>
		/// </summary>
		private void InitializeCommands()
		{
			SearchAsyncCommand = AsyncCommand.Create(() => ExecuteSearchAsync(), () => CanExecuteSearch(null));
			SearchParamsUpdatedCommand = new RelayCommand((o) => SearchAsyncCommand.RaiseCanExecuteChanged(), (o) => !IsSearching);
			OpenLink = new RelayCommand(ExecuteOpenLink, CanExecuteOpenLink);
			SearchParams = new SearchParams();
			UpdateImages = new RelayCommand(ExecuteUpdateImages, CanExecuteUpdateImages);
			PrevImageCommand = AsyncCommand.Create(() => ExecuteUpdatePrevImageAsync(null), () => CanExecuteUpdatePrevImage(null));
			NextImageCommand = AsyncCommand.Create(() => ExecuteUpdateNextImageAsync(null), () => CanExecuteUpdateNextImage(null));
		}

		#region Command functions

		/// <summary>
		/// Can execute update next image
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private bool CanExecuteUpdateNextImage(object obj)
		{
			return RightMovePropertyFullSelectedItem != null && _selectedImageIndex != RightMovePropertyFullSelectedItem.ImageUrl.Length - 1;
		}

		private async Task<BitmapImage> ExecuteUpdateNextImageAsync(object arg1)
		{
			_selectedImageIndex++;
			_tokenSource.Cancel();

			_tokenSource = new CancellationTokenSource();
			var token = _tokenSource.Token;
			var bitmap = await UpdateImage(token);
			return bitmap;
		}

		private bool CanExecuteUpdatePrevImage(object obj)
		{
			if (RightMovePropertyFullSelectedItem is null)
			{
				return false;
			}

			return _selectedImageIndex > 0;
		}

		private async Task<BitmapImage> ExecuteUpdatePrevImageAsync(object arg1)
		{
			_selectedImageIndex--;
			_tokenSource.Cancel();
			_tokenSource = new CancellationTokenSource();

			var token = _tokenSource.Token;
			var bitmap = await UpdateImage(token);
			return bitmap;
		}

		private void ExecuteUpdateImages(object arg)
		{
			if (_selectedItemChangedTimer.IsEnabled)
			{
				_selectedItemChangedTimer.Stop();
			}

			_selectedItemChangedTimer.Start();
			LoadingImage = true;
		}

		/// <summary>
		/// Can execute update images
		/// </summary>
		/// <param name="arg">the argument</param>
		/// <returns>true if can execute, false otherwise</returns>
		private bool CanExecuteUpdateImages(object arg)
		{
			return RightMoveSelectedItem != null;
		}

		/// <summary>
		/// Execute open link command
		/// </summary>
		/// <param name="obj">the object</param>
		private void ExecuteOpenLink(object obj)
		{
			if (RightMoveSelectedItem is null)
			{
				return;
			}

			BrowserHelper.OpenWebpage(RightMoveSelectedItem.Url);
		}

		/// <summary>
		/// Can execute open link command
		/// </summary>
		/// <param name="arg">the argument</param>
		/// <returns>true if can execute, false otherwise</returns>
		private bool CanExecuteOpenLink(object arg)
		{
			return true;
		}

		/// <summary>
		/// The execute search command
		/// </summary>
		/// <param name="parameter"></param>
		// private async Task ExecuteSearchAsync(object parameter)
		private async Task<RightMoveSearchItemCollection> ExecuteSearchAsync()
		{
			IsSearching = true;

			// create a copy if search params in case its changed during search
			SearchParams searchParams = new SearchParams(SearchParams);
			await _rightMoveModel.GetRightMoveItems(searchParams);

			UpdateAveragePrice();

			// add properties to DB
			IsSearching = false;
			return _rightMoveModel.RightMovePropertyItems;
		}

		/// <summary>
		/// The can execute search command
		/// </summary>
		/// <param name="parameter">the parameter</param>
		/// <returns>true if can execute, false otherwise</returns>
		private bool CanExecuteSearch(object parameter)
		{
			return !IsSearching && SearchParams.IsValid();
		}

		#endregion

		private void UpdateAveragePrice()
		{
			if (RightMoveList != null)
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendLine($"Average price: {RightMoveList.AveragePrice.ToString("C2")}");
				sb.Append($"Property count: {RightMoveList.Count}");
				Info = sb.ToString();
			}
			else
			{
				Info = "...";
			}
		}

		private void UpdateImageIndexView()
		{
			ImageIndexView = _selectedImageIndex < 0 || !HasImages
				? null
				: $"Image {_selectedImageIndex + 1} / {RightMovePropertyFullSelectedItem.ImageUrl.Length}";
		}

		private async void SelectedItemChanged_Elapsed(object sender, EventArgs e)
		{
			_selectedItemChangedTimer.Stop();

			try
			{
				_tokenSource.Cancel();

				_tokenSource = new CancellationTokenSource();
				CancellationToken cancellationToken = _tokenSource.Token;

				await UpdateFullSelectedItemAndImage(cancellationToken);
			}
			catch (Exception)
			{
				System.Diagnostics.Debug.WriteLine("Operation exception");
			}
		}
	}
}
