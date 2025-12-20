using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using RightMove.DataTypes;
using static RightMove.Desktop.UserControls.AutoCompleteComboBox;
using static RightMove.DataTypes.SearchParams;

namespace RightMove.Desktop.View
{
	public class SearchParamsViewModel : ObservableRecipient
	{
		public SearchParamsViewModel()
		{
			SearchParams = new SearchParams();
		}

		public event EventHandler SearchParamsUpdated;


		public SearchParams SearchParams
		{
			get => _searchParams;
			set
			{
				if (SetProperty(ref _searchParams, value))
				{
					OnPropertyChanged(nameof(RegionLocation));
					OnPropertyChanged(nameof(Radius));
					OnPropertyChanged(nameof(MinBedrooms));
					OnPropertyChanged(nameof(MaxBedrooms));
					OnPropertyChanged(nameof(MinPrice));
					OnPropertyChanged(nameof(MaxPrice));
					OnPropertyChanged(nameof(PropertyType));
					OnPropertyChanged(nameof(SortType));
				}
			}
		}

		public string SearchText
		{
			get => _searchText;
			set => SetProperty(ref _searchText, value);
		}

		public RightMoveRegion SelectedRightMoveRegion
        {
            get => _selectedRightMoveRegion;
            set
            {
                if (SetProperty(ref _selectedRightMoveRegion, value))
                {
                    SearchParams.RegionLocation = _selectedRightMoveRegion?.Id;
					OnSearchParamsChanged();
                }
            }
        }

        public string RegionLocation
		{
			get => SearchParams.RegionLocation;
			set
			{
				if (SearchParams.RegionLocation != value)
				{
					SearchParams.RegionLocation = value;
					OnSearchParamsChanged();
				}
			}
		}

		public double Radius
		{
			get => SearchParams.Radius;
			set
			{
				if (SearchParams.Radius != value)
				{
					SearchParams.Radius = value;
					OnSearchParamsChanged();
				}
			}
		}

		public int MinBedrooms
		{
			get { return SearchParams.MinBedrooms; }
			set
			{
				if (SearchParams.MinBedrooms != value)
				{
					SearchParams.MinBedrooms = value;
					OnSearchParamsChanged();
				}
			}
		}

		public int MaxBedrooms
		{
			get { return SearchParams.MaxBedrooms; }
			set
			{
				if (SearchParams.MaxBedrooms != value)
				{
					SearchParams.MaxBedrooms = value;
					OnSearchParamsChanged();
				}
			}
		}

		public int MinPrice
		{
			get { return SearchParams.MinPrice; }
			set
			{
				if (SearchParams.MinPrice != value)
				{
					SearchParams.MinPrice = value;
					OnSearchParamsChanged();
				}
			}
		}

		public int MaxPrice
		{
			get { return SearchParams.MaxPrice; }
			set
			{
				if (SearchParams.MaxPrice != value)
				{
					SearchParams.MaxPrice = value;
					OnSearchParamsChanged();
				}
			}
		}

		private PropertyTypeEnum _propertyType;
        private ObservableCollection<string> _regionStrings;
        private RightMoveRegion _selectedRightMoveRegion;
        private AutocompleteSearchCallback _rightMoveFunc = DefaultFunc;
        private string _searchText;
        private SearchParams _searchParams;

        public PropertyTypeEnum PropertyType
		{
			get { return SearchParams.PropertyType; }
			set
			{
				if (SearchParams.PropertyType != value)
				{
					SearchParams.PropertyType = value;
					OnSearchParamsChanged();
				}
			}
		}


		public SortType SortType
		{
			get { return SearchParams.Sort; }
			set
			{
				if (SearchParams.Sort != value)
				{
					SearchParams.Sort = value;
					OnSearchParamsChanged();
				}
			}
		}

        public ObservableCollection<string> RegionStrings
        {
            get => _regionStrings;
            set => SetProperty(ref _regionStrings, value);
        }

        public static AutocompleteSearchCallback DefaultFunc = async (text, token) =>
        {
            var regionService = new RightMoveRegionService();

            try
            {
                var items = (await regionService.SearchAsync(text, token)).ToList();
                return items;
            }
            catch (TaskCanceledException)
            {
                return new List<RightMoveRegion>();
            }
        };

        public AutocompleteSearchCallback RightMoveFunc
        {
            get => _rightMoveFunc;
            set => SetProperty(ref _rightMoveFunc, value);
        }

        /// <summary>
        /// Radius entries bound to combobox
        /// </summary>
        public Dictionary<double, string> RadiusEntries { get; } = new Dictionary<double, string>()
        {
            {0, "This area only" },
            { 0.25, "Within 1/4 mile" },
            { 0.5, "Within 1/2 mile" },
            { 1, "Within 1 mile" },
            { 3, "Within 3 miles" },
            { 5, "Within 5 miles" },
            { 10, "Within 10 miles" },
            { 15, "Within 15 miles" },
            { 20, "Within 20 miles" },
            { 30, "Within 30 miles" },
            { 40, "Within 40 miles" }
        };

        public Dictionary<SortType, string> SortTypes => SortTypeDictionary;

        public Dictionary<PropertyTypeEnum, string> PropertyTypes => PropertyTypeDictionary;

        /// <summary>
        /// Prices bound to combo box
        /// </summary>
        public List<int> Prices { get; } = new List<int>()
        {
            0, 50000, 60000, 70000, 80000, 90000, 100000, 110000, 120000, 125000,
            130000, 150000, 200000, 250000, 300000, 325000, 375000, 400000, 425000,
            450000, 475000, 500000, 550000, 600000, 650000, 700000, 800000, 900000,
            1000000, 1250000, 1500000, 1750000, 2000000, 2500000, 3000000, 4000000,
            5000000, 7500000, 10000000, 15000000, 20000000
        };

        /// <summary>
        /// Bedrooms bound to combobox
        /// </summary>
        public List<int> Bedrooms { get; } = new List<int>()
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
        };

        public List<string> SearchString => RightMoveCodes.RegionTree;

        public void OnSearchParamsChanged()
		{
			SearchParamsUpdated?.Invoke(this, new EventArgs());
		}

	}
}
