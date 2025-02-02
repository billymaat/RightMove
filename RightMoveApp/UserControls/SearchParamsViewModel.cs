using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RightMove.DataTypes;

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
			get;
			set;
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


        public void OnSearchParamsChanged()
		{
			SearchParamsUpdated?.Invoke(this, new EventArgs());
		}

	}
}
