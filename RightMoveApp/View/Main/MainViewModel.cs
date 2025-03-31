using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Globalization;
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
using ServiceCollectionUtilities;
using RelayCommand = RightMove.Desktop.ViewModel.Commands.RelayCommand;

namespace RightMove.Desktop.View.Main
{
	public class MainViewModel : ObservableRecipient
	{
		// Services
		private readonly NavigationService _navigationService;
		private readonly IMessenger _messenger;
		private readonly IFactory<RightMoveModel> _rightMoveModelFactory;
		private readonly IFactory<SearchResultsViewModel> _searchResultsViewModelFactory;

		// The right move model
		private readonly SearchHistoryService _searchHistoryService;
		private readonly AppSettings _settings;
		private SearchParamsViewModel _searchParamsViewModel;

		private ILogger<MainViewModel> _logger;

        public MainViewModel(IOptions<AppSettings> settings,
			SearchHistoryService searchHistoryService,
			NavigationService navigationService,
			IMessenger messenger,
			IFactory<RightMoveModel> rightMoveModelFactory,
			IFactory<SearchResultsViewModel> searchResultsViewModelFactory,
			ILogger<MainViewModel> logger)
		{
            _logger = logger;

			_logger.LogInformation("MainViewModel loaded");

			_settings = settings.Value;
			_navigationService = navigationService;
			_messenger = messenger;
			_rightMoveModelFactory = rightMoveModelFactory;
			_searchResultsViewModelFactory = searchResultsViewModelFactory;

			InitializeCommands();

			_searchHistoryService = searchHistoryService;
			IsSearching = false;

            _searchParamsViewModel = new SearchParamsViewModel();
            //{
            //    RegionLocation = "ashton-under-lyne"
            //};

			TopViewModel = _searchParamsViewModel;
			_searchParamsViewModel.SearchParamsUpdated += OnSearchParamsChanged;

			SearchParamsHistory = new ObservableCollection<SearchHistoryItem>(_searchHistoryService.GetItems());
			
			
            messenger.Register<SearchHistoryItemsUpdatedMessage>(this, (recipient, message) => SearchParamsHistory = new ObservableCollection<SearchHistoryItem>(message.NewValue));

			SearchResults = new ObservableCollection<SearchResultsViewModel>();

			SearchItemDoubleClickCommand = new RelayCommand<SearchHistoryItem>(ExecuteSearchItemDoubleClick, CanExecuteSearchItemDoubleClick);
		}

        private ICommand _searchItemDoubleClickCommand;

        public ICommand SearchItemDoubleClickCommand
        {
	        get => _searchItemDoubleClickCommand;
	        set => SetProperty(ref _searchItemDoubleClickCommand, value);
        }

        public ObservableCollection<SearchResultsViewModel> SearchResults
        {
	        get => _searchResults;
	        set => SetProperty(ref _searchResults, value);
        }

        public SearchResultsViewModel SelectedSearchResults
        {
	        get => _selectedSearchResults;
	        set => SetProperty(ref _selectedSearchResults, value);
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

        private ObservableRecipient _topViewModel;

		public ObservableRecipient TopViewModel
		{
			get => _topViewModel;
            set => SetProperty(ref _topViewModel, value);
        }

		public bool IsImagesVisible
		{
			get => _isImagesVisible;
			set => SetProperty(ref _isImagesVisible, value);
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
		private ObservableCollection<SearchHistoryItem> _searchParamsHistory;
        private bool _isImagesVisible;
        private IAsyncRelayCommand _searchAsyncCommand;
        private ICommand _loadImageWindow;
        private ObservableCollection<SearchResultsViewModel> _searchResults;
        private SearchResultsViewModel _selectedSearchResults;

        public bool HasSearchedExecuted
		{
			get => _hasSearchExecuted;
			set => SetProperty(ref _hasSearchExecuted, value);
		}

		#region Commands

		/// <summary>
		/// Gets or sets the search command
		/// </summary>
		public IAsyncRelayCommand SearchAsyncCommand
		{
			get => _searchAsyncCommand;
			set => SetProperty(ref _searchAsyncCommand, value);
		}

		public ICommand LoadImageWindow
		{
			get => _loadImageWindow;
			set => SetProperty(ref _loadImageWindow, value);
		}

		#endregion

		/// <summary>
		/// Initialize a bunch of <see cref="ICommand"/>
		/// </summary>
		private void InitializeCommands()
		{
			SearchAsyncCommand = new AsyncRelayCommand(ExecuteSearchAsync, CanExecuteSearch);
        }

        #region Command functions

		/// <summary>
		/// The execute search command
		/// </summary>
		// private async Task ExecuteSearchAsync(object parameter)
		private async Task ExecuteSearchAsync()
		{
			IsSearching = true;

			// create a copy if search params in case its changed during search
			SearchParams searchParams = new SearchParams(_searchParamsViewModel.SearchParams);
			var rightMoveModel = _rightMoveModelFactory.Create();
			var guid = Guid.NewGuid();
			rightMoveModel.SetToken(guid.ToString());
			var searchResultsViewModel = _searchResultsViewModelFactory.Create();
			searchResultsViewModel.SetRightMoveModel(rightMoveModel);
			searchResultsViewModel.SetToken(guid.ToString());
			searchResultsViewModel.SetLocation(_searchParamsViewModel.SearchText);

			SearchResults.Add(searchResultsViewModel);
			await rightMoveModel.Search(searchParams, _searchParamsViewModel.SearchText);

			SelectedSearchResults = searchResultsViewModel;
			//UpdateAveragePrice();

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
		
	}
}
