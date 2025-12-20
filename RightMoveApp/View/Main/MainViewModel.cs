using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Pkcs;
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
using RightMove.Desktop.Factory;
using RightMove.Desktop.Model;
using RightMove.Desktop.Services;
using RightMove.Desktop.ViewModel;
using RightMove.Extensions;

namespace RightMove.Desktop.View.Main
{
	public class MainViewModel : ObservableRecipient
	{
		// Services
		private readonly NavigationService _navigationService;
		private readonly SearchRightMoveService _searchRightMoveService;
		private readonly ResultsTabViewModelFactory _resultsTabViewModelFactory;

		// Backing fields
		private string _info;

		// The right move model
		private readonly SearchHistoryService _searchHistoryService;
		private readonly AppSettings _settings;


        private ILogger<MainViewModel> _logger;

        public MainViewModel(IOptions<AppSettings> settings,
			SearchHistoryService searchHistoryService,
			SearchHistoryModel searchHistoryModel,
			NavigationService navigationService,
			SearchRightMoveService searchRightMoveService,
			ResultsTabViewModelFactory resultsTabViewModelFactory,
			IMessenger messenger,
			ILogger<MainViewModel> logger)
        {
	        _logger = logger;

			_logger.LogInformation("MainViewModel loaded");

			_settings = settings.Value;
			_navigationService = navigationService;
			_searchRightMoveService = searchRightMoveService;
			_resultsTabViewModelFactory = resultsTabViewModelFactory;
			_searchHistoryService = searchHistoryService;

			SearchItemDoubleClickCommand = new RelayCommand<SearchHistoryItem>(ExecuteSearchItemDoubleClick, CanExecuteSearchItemDoubleClick);
			SearchAsyncCommand = new AsyncRelayCommand(ExecuteSearchAsync, CanExecuteSearch);

			IsSearching = false;

			SearchParamsViewModel = new SearchParamsViewModel();
			SearchParamsViewModel.SearchParamsUpdated += OnSearchParamsChanged;

			SearchParamsHistory = new ObservableCollection<SearchHistoryItem>(_searchHistoryService.GetItems());
			searchHistoryModel.SearchHistoryItemsUpdated += (recipient, message) => SearchParamsHistory = new ObservableCollection<SearchHistoryItem>(message.SearchHistoryItems);
			ResultsTabViewModels = new ObservableCollection<ResultsTabViewModel>();
        }

        public ObservableCollection<ResultsTabViewModel> ResultsTabViewModels
        {
	        get => _resultsTabViewModels;
	        set => SetProperty(ref _resultsTabViewModels, value);
        }

        /// <summary>
        /// Gets the Loading text in the busy spinner
        /// </summary>
        public string Text => "Loading...";

        private void OnSearchParamsChanged(object sender, EventArgs e)
		{
			SearchAsyncCommand.NotifyCanExecuteChanged();
		}

        private SearchParamsViewModel _searchParamsViewModel;

		public SearchParamsViewModel SearchParamsViewModel
		{
			get => _searchParamsViewModel;
            set => SetProperty(ref _searchParamsViewModel, value);
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
        private ObservableCollection<RightMoveProperty> _rightMovePropertyItems;
        private ObservableCollection<SearchHistoryItem> _searchParamsHistory;
        private ObservableCollection<ResultsTabViewModel> _resultsTabViewModels;
        private ResultsTabViewModel _selectedResultsTabViewModel;

        public bool HasSearchedExecuted
		{
			get => _hasSearchExecuted;
			set => SetProperty(ref _hasSearchExecuted, value);
		}

		#region Commands

		public ICommand SearchItemDoubleClickCommand
		{
			get;
		}
		/// <summary>
		/// Gets or sets the search command
		/// </summary>
		public IAsyncRelayCommand SearchAsyncCommand
		{
			get;
		}


		public PropertyInfoViewModel PropertyInfoViewModel { get; }
		public RightMoveImageViewModel RightMoveImageViewModel { get; }

		public ResultsTabViewModel SelectedResultsTabViewModel
		{
			get => _selectedResultsTabViewModel;
			set => SetProperty(ref _selectedResultsTabViewModel, value);
		}

		#endregion

		private bool CanExecuteSearchItemDoubleClick(SearchHistoryItem arg)
		{
			return true;
		}

		private void ExecuteSearchItemDoubleClick(SearchHistoryItem obj)
		{
			SearchParamsViewModel.SearchParams = obj.SearchParams;
			SearchParamsViewModel.SearchText = obj.DisplayText;
			SearchAsyncCommand.NotifyCanExecuteChanged();
		}
		
        #region Command functions



		/// <summary>
		/// The execute search command
		/// </summary>
		/// <param name="parameter"></param>
		// private async Task ExecuteSearchAsync(object parameter)
		private async Task ExecuteSearchAsync()
		{
			IsSearching = true;

			// create a copy if search params in case its changed during search
			SearchParams searchParams = new SearchParams(SearchParamsViewModel.SearchParams);
			var ret = await _searchRightMoveService.Search(searchParams, SearchParamsViewModel.SearchText);

			var resultsTabViewModel = _resultsTabViewModelFactory.Create(ret, SearchParamsViewModel.SearchText);
			ResultsTabViewModels.Add(resultsTabViewModel);
			SelectedResultsTabViewModel = resultsTabViewModel;

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
			return !IsSearching && SearchParamsViewModel.SearchParams.IsValid();
		}

		#endregion
	}
}
