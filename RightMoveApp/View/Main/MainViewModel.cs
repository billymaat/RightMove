using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RightMove.DataTypes;
using RightMove.Desktop.Helpers;
using RightMove.Desktop.Messages;
using RightMove.Desktop.Model;
using RightMove.Desktop.Services;
using RightMove.Desktop.UserControls;
using RightMove.Desktop.ViewModel;
using RightMove.Desktop.ViewModel.Commands;
using RightMove.Extensions;

using RelayCommand = RightMove.Desktop.ViewModel.Commands.RelayCommand;

namespace RightMove.Desktop.View.Main
{
	public class MainViewModel : ObservableRecipient
	{
		// Services
		private readonly NavigationService _navigationService;

		// Backing fields
		private string _info;

		// cancellation token
		private CancellationTokenSource _tokenSource = new CancellationTokenSource();

		// Time for selected item changed in data grid
		private System.Windows.Threading.DispatcherTimer _selectedItemChangedTimer;

		// The right move model
		private readonly RightMoveModel _rightMoveModel;
		private readonly SearchHistoryService _searchHistoryService;
		private readonly AppSettings _settings;
		private SearchParamsViewModel _searchParamsViewModel;


        private readonly PropertyInfoViewModel _propertyInfoViewModel;
        private ILogger<MainViewModel> _logger;

        public MainViewModel(IOptions<AppSettings> settings,
			PropertyInfoViewModel propertyInfoViewModel,
			RightMoveModel rightMoveModel,
			SearchHistoryService searchHistoryService,
			NavigationService navigationService,
			IMessenger messenger,
			ILogger<MainViewModel> logger)
		{
            _propertyInfoViewModel = propertyInfoViewModel;
            _logger = logger;

			_logger.LogInformation("MainViewModel loaded");

			_settings = settings.Value;
			_navigationService = navigationService;

            InitializeCommands();
			InitializeTimers();

			_rightMoveModel = rightMoveModel;
			_searchHistoryService = searchHistoryService;
			IsSearching = false;

            _searchParamsViewModel = new SearchParamsViewModel();
            //{
            //    RegionLocation = "ashton-under-lyne"
            //};

			TopViewModel = _searchParamsViewModel;
			_searchParamsViewModel.SearchParamsUpdated += OnSearchParamsChanged;

			SearchParamsHistory = new ObservableCollection<SearchHistoryItem>(_searchHistoryService.GetItems());

			messenger.Register<RightMoveSelectedItemUpdatedMessage>(this, (recipient, message) => RightMoveSelectedItem = message.NewValue);
            messenger.Register<RightMoveFullSelectedItemUpdatedMessage>(this, (recipient, message) => RightMovePropertyFullSelectedItem = message.NewValue);
            messenger.Register<RightMovePropertyItemsUpdatedMessage>(this, (recipient, message) => RightMovePropertyItems = new ObservableCollection<RightMoveProperty>(message.NewValue));
            messenger.Register<SearchHistoryItemsUpdatedMessage>(this, (recipient, message) => SearchParamsHistory = new ObservableCollection<SearchHistoryItem>(message.NewValue));
		}

		/// <summary>
		/// Gets the Loading text in the busy spinner
		/// </summary>
        public string Text => "Loading...";

        private void OnSearchParamsChanged(object sender, EventArgs e)
		{
			SearchAsyncCommand.NotifyCanExecuteChanged();
		}

        private void RightMoveModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			//if (e.PropertyName == nameof(_rightMoveModel.RightMovePropertyItems))
			//{
			//	OnPropertyChanged(nameof(RightMovePropertyItems));
			//}
			//else if (e.PropertyName == nameof(_rightMoveModel.RightMovePropertyFullSelectedItem))
			//{
			//	OnPropertyChanged(nameof(RightMovePropertyFullSelectedItem));
			//}
		}

		private ObservableRecipient _topViewModel;

		public ObservableRecipient TopViewModel
		{
			get => _topViewModel;
            set => SetProperty(ref _topViewModel, value);
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
			set => SetProperty(ref _info, value);
		}

        /// <summary>
        /// Gets or sets the right move items
        /// </summary>
        public ObservableCollection<RightMoveProperty> RightMovePropertyItems
        {
            get => _rightMovePropertyItems;
            set => SetProperty(ref _rightMovePropertyItems, value);
        }

        private RightMoveProperty _rightMoveSelectedItem;
		/// <summary>
		/// Gets or sets the selected <see cref="RightMoveViewItem"/>
		/// </summary>
		public RightMoveProperty RightMoveSelectedItem
		{
			get => _rightMoveSelectedItem;
			set => SetProperty(ref _rightMoveSelectedItem, value);
		}

        public RightMoveProperty RightMovePropertyFullSelectedItem
        {
            get => _rightMovePropertyFullSelectedItem;
            set => SetProperty(ref _rightMovePropertyFullSelectedItem, value);
        }

        public ObservableCollection<SearchHistoryItem> SearchParamsHistory
        {
	        get => _searchParamsHistory;
	        set => SetProperty(ref _searchParamsHistory, value);
        }

        public List<int> Prices
		{
			get
			{
				return null;

				//if (RightMoveSelectedItem is null)
				//{
				//	return null;
				//}

				//var dbProperties = _dbService.LoadProperties();
				//var matchingProperty = dbProperties.FirstOrDefault(o => o.RightMoveId.Equals(RightMoveSelectedItem.RightMoveId));

				//if (matchingProperty is null)
				//{
				//	return null;
				//}

				//var prices = matchingProperty.Prices;

				//return prices;
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

				return na;
				//if (RightMoveSelectedItem is null)
				//{
				//	return na;
				//}

				//var dbProperties = _dbService.LoadProperties();
				//var matchingProperty = dbProperties.FirstOrDefault(o => o.RightMoveId.Equals(RightMoveSelectedItem.RightMoveId));

				//if (matchingProperty is null)
				//{
				//	return na;
				//}

				//var dates = matchingProperty.Dates;
				//var prices = matchingProperty.Prices;

				//var combined = dates.Zip(prices, (d, p) => $"{DateTime.Parse(d).Date.ToString("dd/MM/yyyy")} : £{p}");
				//var priceString = string.Join("\n", combined);
				//return priceString;
			}
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
				SetProperty(ref _isSearching, value);
				//SearchAsyncCommand.RaiseCanExecuteChanged();
			}
		}

		private bool _hasSearchExecuted;
        private RightMoveProperty _rightMovePropertyFullSelectedItem;
        private ObservableCollection<RightMoveProperty> _rightMovePropertyItems;
        private ObservableCollection<SearchHistoryItem> _searchParamsHistory;

        public bool HasSearchedExecuted
		{
			get => _hasSearchExecuted;
			set => SetProperty(ref _hasSearchExecuted, value);
		}

		#region Commands

		public ICommand SearchItemDoubleClickCommand
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the search command
		/// </summary>
		public IAsyncRelayCommand SearchAsyncCommand
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

        public ICommand SelectionChangedCommand
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
			SearchItemDoubleClickCommand = new RelayCommand<SearchHistoryItem>(ExecuteSearchItemDoubleClick, CanExecuteSearchItemDoubleClick);
			SearchAsyncCommand = new AsyncRelayCommand(ExecuteSearchAsync, CanExecuteSearch);
			OpenLink = new RelayCommand(ExecuteOpenLink, CanExecuteOpenLink);
            SelectionChangedCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand<RightMoveProperty>(ExecuteSelectionChanged, (obj) => true);
        }

		private bool CanExecuteSearchItemDoubleClick(SearchHistoryItem arg)
		{
			return true;
		}

		private void ExecuteSearchItemDoubleClick(SearchHistoryItem obj)
		{
			_searchParamsViewModel.SearchParams = obj.SearchParams;
			_searchParamsViewModel.SearchText = obj.DisplayText;
			SearchAsyncCommand.NotifyCanExecuteChanged();
		}

		private async Task ExecuteSelectionChanged(RightMoveProperty rightMoveProperty)
        {
            if (rightMoveProperty == null)
            {
                return;
            }

			// need to parse the full image
            await _rightMoveModel.UpdateSelectedRightMoveItem(rightMoveProperty.RightMoveId, _tokenSource.Token);
        }


        #region Command functions

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
		private async Task ExecuteSearchAsync()
		{
			IsSearching = true;

			// create a copy if search params in case its changed during search
			SearchParams searchParams = new SearchParams(_searchParamsViewModel.SearchParams);
			await _rightMoveModel.Search(searchParams, _searchParamsViewModel.SearchText);

			UpdateAveragePrice();

			// add properties to DB
			IsSearching = false;
		}

		/// <summary>
		/// The can execute search command
		/// </summary>
		/// <param name="parameter">the parameter</param>
		/// <returns>true if can execute, false otherwise</returns>
		private bool CanExecuteSearch()
		{
			return !IsSearching && _searchParamsViewModel.SearchParams.IsValid();
		}

		#endregion

		private void UpdateAveragePrice()
		{
			if (RightMovePropertyItems != null)
			{
				StringBuilder sb = new StringBuilder();

                var averagePrice = RightMovePropertyItems.AveragePrice();
				if (averagePrice != double.MinValue)
				{
					sb.AppendLine($"Average price: {averagePrice.ToString("C2")}");
				}
				sb.Append($"Property count: {RightMovePropertyItems.Count}");
				Info = sb.ToString();
			}
			else
			{
				Info = "...";
			}
		}



		private async void SelectedItemChanged_Elapsed(object sender, EventArgs e)
		{
			_selectedItemChangedTimer.Stop();

			try
			{
				_tokenSource.Cancel();

				_tokenSource = new CancellationTokenSource();
				CancellationToken cancellationToken = _tokenSource.Token;

				//await UpdateFullSelectedItemAndImage(cancellationToken);
			}
			catch (Exception)
			{
				System.Diagnostics.Debug.WriteLine("Operation exception");
			}
		}
	}
}
