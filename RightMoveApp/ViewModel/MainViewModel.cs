using GalaSoft.MvvmLight;
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
		private System.Timers.Timer _selectedItemChangedTimer;

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

		/// <summary>
		/// Gets or sets the <see cref="SearchParams"/>
		/// </summary>
		public SearchParams SearchParams
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the displayed image
		/// </summary>
		public BitmapImage DisplayedImage
		{
			get => _displayedImage;
			set => Set(ref _displayedImage, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether searching is occurring
		/// </summary>
		public bool IsSearching { get; set; }

		/// <summary>
		/// Gets a value indicating whether selected item has images available
		/// </summary>
		public bool HasImages => RightMovePropertyFullSelectedItem != null && RightMovePropertyFullSelectedItem.ImageUrl.Length > 0;

		public string ImageIndexView
		{
			get => _imageIndexView;
			set => Set(ref _imageIndexView, value);
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
			await UpdateImage(RightMovePropertyFullSelectedItem, _selectedImageIndex, cancellationToken);
			LoadingImage = false;
		}

		private async Task UpdateRightMovePropertyFullSelectedItem(CancellationToken cancellationToken)
		{
			_selectedImageIndex = 0;

			// update the right move full selected item
			await _rightMoveModel.GetFullRightMoveItem(RightMoveSelectedItem.RightMoveId, cancellationToken);

			// refresh the can execute of image commands
			PrevImageCommand.RaiseCanExecuteChanged();
			NextImageCommand.RaiseCanExecuteChanged();
		}

		private async Task<BitmapImage> UpdateImage(RightMoveProperty rightMoveProperty, int selectedIndex, CancellationToken cancellationToken)
		{
			try
			{
				byte[] imageArr = await rightMoveProperty.GetImage(selectedIndex);
				if (imageArr is null)
				{
					return null;
				}

				if (cancellationToken.IsCancellationRequested)
				{
					cancellationToken.ThrowIfCancellationRequested();
				}

				var bitmapImage = ImageHelper.ToImage(imageArr);

				// freeze as accessed from non UI thread
				bitmapImage.Freeze();

				DisplayedImage = bitmapImage;

				// update the image view
				UpdateImageIndexView();
				return bitmapImage;
			}
			catch (OperationCanceledException e)
			{
				Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
				return null;
			}
		}

		private void InitializeTimers()
		{
			_selectedItemChangedTimer = new System.Timers.Timer(500);
			_selectedItemChangedTimer.Elapsed += SelectedItemChanged_Elapsed;
		}

		/// <summary>
		/// Initialize a bunch of <see cref="ICommand"/>
		/// </summary>
		private void InitializeCommands()
		{
			SearchAsyncCommand = AsyncCommand.Create(() => ExecuteSearchAsync(), () => CanExecuteSearch(null));
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
			var bitmap = await UpdateImage(RightMovePropertyFullSelectedItem, _selectedImageIndex, token);
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
			var bitmap = await UpdateImage(RightMovePropertyFullSelectedItem, _selectedImageIndex, token);
			return bitmap;
		}

		private void ExecuteUpdateImages(object arg)
		{
			System.Diagnostics.Debug.WriteLine(RightMoveSelectedItem.RightMoveId);

			if (_selectedItemChangedTimer.Enabled)
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
			return SearchParams.IsValid();
		}

		#endregion

		private void UpdateAveragePrice()
		{
			string info;
			if (RightMoveList != null)
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendLine($"Average price: {RightMoveList.AveragePrice.ToString("C2")}");
				sb.Append($"Count: {RightMoveList.Count}");
				info = sb.ToString();
			}
			else
			{
				info = "...";
			}

			Info = info;
		}

		private void UpdateImageIndexView()
		{
			if (_selectedImageIndex < 0 || !HasImages)
			{
				ImageIndexView = null;
			}
			else
			{
				ImageIndexView = $"Images: {_selectedImageIndex + 1} / {RightMovePropertyFullSelectedItem.ImageUrl.Length}";
			}
		}

		private void SelectedItemChanged_Elapsed(object sender, ElapsedEventArgs e)
		{
			_selectedItemChangedTimer.Stop();

			try
			{
				CancellationToken cancellationToken = _tokenSource.Token;
				Task.Run(async () => await UpdateFullSelectedItemAndImage(cancellationToken), cancellationToken);
			}
			catch (Exception)
			{
				System.Diagnostics.Debug.WriteLine("Operation exception");
			}
		}
	}
}
