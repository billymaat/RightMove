using GalaSoft.MvvmLight;
using Microsoft.Extensions.DependencyInjection;
using RightMove.DataTypes;
using RightMove.Factory;
using RightMove.Services;
using RightMoveApp.Helpers;
using RightMoveApp.Model;
using RightMoveApp.Services;
using RightMoveApp.ViewModel.Commands;
using System;
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
		private readonly IDatabaseService _dbService;
		
		private RightMoveSearchItemCollection _rightMoveList;
		private string _info;

		private int _selectedImageIndex;
		private BitmapImage _displayedImage;
		private RightMoveProperty _rightMovePropertyFullSelectedItem;
		CancellationTokenSource _tokenSource = new CancellationTokenSource();
		private System.Timers.Timer _selectedItemChangedTimer;
		private RightMoveModel _model;

		private RightMoveParserServiceFactory _parserFactory;

		// public MainViewModel(IServiceProvider serviceProvider, NavigationService navigationService, IDatabaseService dbService)
		public MainViewModel(RightMoveParserServiceFactory parserFactory, NavigationService navigationService, IDatabaseService dbService)
		{
			_parserFactory = parserFactory;
			_navigationService = navigationService;
			_dbService = dbService;
			
			InitializeCommands();
			InitializeTimers();

			_model = new RightMoveModel();
			
			IsNotSearching = true;
		}

		public bool IsImagesVisible
		{
			get;
			set;
		}
		
		public string Info
		{
			get =>_info;
			set => Set(ref _info, value);
		}

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
		public bool IsNotSearching { get; set; }

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
		private void ExecuteSearch(object parameter)
		{
			IsNotSearching = false;

			// RightMoveParserService parser = _parserFactory.GetRequiredService<RightMoveParserService>();
			RightMoveParserService parser = _parserFactory.CreateInstance(null);
			Task task = parser.SearchAsync();
			task.ContinueWith(t =>
				{ 
					RightMoveList = parser.Results;
					IsNotSearching = true;
				});
			
			/*
			RightMoveList = parser.Results;
			_dbService.SaveProperties(RightMoveList.ToList());
			
			Info = $"Average price: {parser.Results.AveragePrice.ToString("C2")}";
			*/

			IsNotSearching = true;
		}

		/// <summary>
		/// The execute search command
		/// </summary>
		/// <param name="parameter"></param>
		// private async Task ExecuteSearchAsync(object parameter)
		private async Task<RightMoveSearchItemCollection> ExecuteSearchAsync()
		{
			IsNotSearching = false;

			var parser = _parserFactory.CreateInstance(SearchParams);
			// parser.SearchParams = SearchParams;

			// run the search on a separate context so as not to block the UI thread
			// await parser.SearchAsync().ConfigureAwait(false);
			Task t = Task.Run(() => parser.SearchAsync());

			t.ContinueWith((x) =>
			{
				Info = $"Average price: {parser.Results.AveragePrice.ToString("C2")}";

				IsNotSearching = true;
				RightMoveList = parser.Results;
			});
			
			// _dbService.SaveProperties(RightMoveList.ToList());
			/*
			Info = $"Average price: {parser.Results.AveragePrice.ToString("C2")}";

			IsNotSearching = true;
			RightMoveList = parser.Results;
			*/
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
