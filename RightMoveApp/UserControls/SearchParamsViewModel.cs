﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using RightMove.DataTypes;
using static RightMove.Desktop.UserControls.AutoCompleteComboBox;

namespace RightMove.Desktop.UserControls
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


        public void OnSearchParamsChanged()
		{
			SearchParamsUpdated?.Invoke(this, new EventArgs());
		}

	}
}
