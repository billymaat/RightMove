using GalaSoft.MvvmLight;
using RightMove.DataTypes;
using RightMove.Db.Models;
using RightMove.Db.Repositories;
using RightMove.Factory;
using RightMoveApp.Helpers;
using RightMoveApp.Model;
using RightMoveApp.Services;
using RightMoveApp.ViewModel.Commands;
using System;
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
		private readonly IRightMovePropertyRepository _dbService;
		
		private string _info;

		private int _selectedImageIndex;
		private BitmapImage _displayedImage;
		private RightMoveProperty _rightMovePropertyFullSelectedItem;
		CancellationTokenSource _tokenSource = new CancellationTokenSource();
		private System.Timers.Timer _selectedItemChangedTimer;
		private RightMoveModel _model;

		private RightMoveParserServiceFactory _parserFactory;

		public MainViewModel(RightMoveParserServiceFactory parserFactory, NavigationService navigationService, IRightMovePropertyRepository dbService)
		{
			_parserFactory = parserFactory;
			_navigationService = navigationService;
			_dbService = dbService;
			
			InitializeCommands();
			InitializeTimers();

			_model = new RightMoveModel();
			
			IsSearching = false;
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
			get =>_info;
			set => Set(ref _info, value);
		}

		/// <summary>
		/// Gets or sets the right move items
		/// </summary>
		public RightMoveSearchItemCollection RightMoveList
		{
			get => _model.RightMovePropertyItems;
			set
			{
				if (_model.RightMovePropertyItems != value)
				{
					_model.RightMovePropertyItems = value;
					RaisePropertyChanged();
				}
			}
		}

		/// <summary>
		/// Gets or sets the selected <see cref="RightMoveViewItem"/>
		/// </summary>
		public RightMoveProperty RightMoveSelectedItem
		{
			get;
			set;
		}

		public RightMoveProperty RightMovePropertyFullSelectedItem
		{
			get => _rightMovePropertyFullSelectedItem;
			set
			{
				Set(ref _rightMovePropertyFullSelectedItem, value);
				PrevImageCommand.RaiseCanExecuteChanged();
				NextImageCommand.RaiseCanExecuteChanged();
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
		public RelayCommand NextImageCommand
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the PrevImageCommand
		/// </summary>
		public RelayCommand PrevImageCommand
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

		private async Task UpdateFullSelectedItemAndImage(CancellationToken cancellationToken)
		{
			await UpdateRightMovePropertyFullSelectedItem(cancellationToken);
			UpdateImage(_selectedImageIndex, cancellationToken);
		}

		private void UpdateImage(int selectedIndex, CancellationToken cancellationToken)
		{
			try
			{
				UpdateImage(RightMovePropertyFullSelectedItem, selectedIndex, cancellationToken);
			}
			catch (OperationCanceledException e)
			{
				Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
			}
		}
		
		private async Task UpdateRightMovePropertyFullSelectedItem(CancellationToken cancellationToken)
		{
			//_selectedImageIndex = 0;
			//PropertyPageParser parser = _parserFactory.GetRequiredService<PropertyPageParser>();
			//parser.PropertyId = RightMoveSelectedItem.RightMoveId;
			
			//await parser.ParseRightMovePropertyPageAsync(cancellationToken);
			//if (cancellationToken.IsCancellationRequested)
			//{
			//	cancellationToken.ThrowIfCancellationRequested();
			//}

			//var dispatcher = Application.Current.Dispatcher;

			//Action setRightMoveProp = () => RightMovePropertyFullSelectedItem = parser.RightMoveProperty;
			//if (dispatcher.CheckAccess())
			//{
			//	setRightMoveProp();
			//}
			//else
			//{
			//	dispatcher.Invoke(setRightMoveProp);
			//}
		}

		private void UpdateImage(RightMoveProperty rightMoveProperty, int selectedIndex, CancellationToken cancellationToken)
		{
			byte[] imageArr = rightMoveProperty.GetImage(selectedIndex);
			if (imageArr is null)
			{
				return;
			}

			if (cancellationToken.IsCancellationRequested)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}
			
			var bitmapImage = ImageHelper.ToImage(imageArr);
			
			// freeze as accessed from non UI thread
			bitmapImage.Freeze();
			
			DisplayedImage = bitmapImage;
		}

		#endregion

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
			// SearchAsyncCommand = new AsyncCommand<object>(() => ExecuteSearchAsync());// , CanExecuteSearch);
			SearchAsyncCommand = AsyncCommand.Create(() => ExecuteSearchAsync());
			// SearchAsyncCommand = new RelayCommand(ExecuteSearch, CanExecuteSearch);
			OpenLink = new RelayCommand(ExecuteOpenLink, CanExecuteOpenLink);
			// LoadImageWindow = new AsyncCommand<object>(ExecuteLoadImageWindowAsync);//, CanExecuteLoadImageWindow);
			SearchParams = new SearchParams();
			UpdateImages = new RelayCommand(ExecuteUpdateImages, CanExecuteUpdateImages);
			PrevImageCommand = new RelayCommand(ExecuteUpdatePrevImageAsync, CanExecuteUpdatePrevImage);
			NextImageCommand = new RelayCommand(ExecuteUpdateNextImageAsync, CanExecuteUpdateNextImage);
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

		private void ExecuteUpdateNextImageAsync(object arg1)
		{
			_selectedImageIndex++;
			CancellationTokenSource tokenSource = new CancellationTokenSource();
			var token = tokenSource.Token;
			UpdateImage(RightMovePropertyFullSelectedItem, _selectedImageIndex, token);
		}

		private bool CanExecuteUpdatePrevImage(object obj)
		{
			if (RightMovePropertyFullSelectedItem is null)
			{
				return false;
			}

			return _selectedImageIndex > 0;
		}

		private void ExecuteUpdatePrevImageAsync(object arg1)
		{
			_selectedImageIndex--;
			CancellationTokenSource tokenSource = new CancellationTokenSource();
			var token = tokenSource.Token;
			UpdateImage(RightMovePropertyFullSelectedItem, _selectedImageIndex, token);
		}

		private void ExecuteUpdateImages(object arg)
		{
			System.Diagnostics.Debug.WriteLine(RightMoveSelectedItem.RightMoveId);

			if (_selectedItemChangedTimer.Enabled)
			{
				_selectedItemChangedTimer.Stop();
			}

			_selectedItemChangedTimer.Start();
		}

		/// <summary>
		/// Can execute update images
		/// </summary>
		/// <param name="arg">the argument</param>
		/// <returns>true if can execute, false otherwise</returns>
		private bool CanExecuteUpdateImages(object arg)
		{
			return IsImagesVisible && RightMoveSelectedItem != null;
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
			var parser = _parserFactory.CreateInstance(searchParams);
			await parser.SearchAsync();
			RightMoveList = parser.Results;

			UpdateAveragePrice();

			UpdateDatabase();

			// add properties to DB
			IsSearching = false;
			return parser.Results;
		}
		
		/// <summary>
		/// The can execute search command
		/// </summary>
		/// <param name="parameter">the parameter</param>
		/// <returns>true if can execute, false otherwise</returns>
		private bool CanExecuteSearch(object parameter)
		{
			return true;
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

		private void UpdateDatabase()
		{
			var dbProperties = _dbService.LoadProperties();
			for (int i = 0; i < RightMoveList.Count; i++)
			{
				var property = RightMoveList[i];

				if (property.RightMoveId == 114594365)
				{

				}
				var matchingProperty = dbProperties.FirstOrDefault(o => o.RightMoveId.Equals(property.RightMoveId));

				if (matchingProperty != null)
				{
					// if the price has changed, add the new price
					if (matchingProperty.Prices.Last() != property.Price)
					{
						_dbService.AddPriceToProperty(matchingProperty.Id, property.Price);
					}
				}
				else
				{
					// save a new record of the new property
					_dbService.SaveProperty(new RightMovePropertyModel(property));
				}
			}
		}

		private void SelectedItemChanged_Elapsed(object sender, ElapsedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("Started running");
			_selectedItemChangedTimer.Stop();

			try
			{
				CancellationToken cancellationToken = _tokenSource.Token;
				Task.Run(async() => await UpdateFullSelectedItemAndImage(cancellationToken), cancellationToken);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Operation exception");
			}
		}
	}
}
