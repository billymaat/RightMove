using GalaSoft.MvvmLight;
using RightMove.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace RightMoveApp.ViewModel
{
	public class SearchParamsControlViewModel : ViewModelBase
	{
		private double _radius;
		private int _minPrice;
		private int _maxPrice;
		private int _minBedrooms;
		private int _maxBedrooms;
		private SortType _sortType;
		private PropertyTypeEnum _propertyType;
		private string _regionLocation;

		public SearchParamsControlViewModel()
		{
			SearchParams = new SearchParams();
		}

		public SearchParams SearchParams
		{
			get;
			set;
		}

		public string RegionLocation
		{
			get => _regionLocation;
			set => Set(ref _regionLocation, value);
		}
		public double Radius
		{
			get => _radius;
			set 
			{
				// Set(ref _radius, value); 
				if (_radius != value)
				{
					RaisePropertyChanged();
					SearchParams.Radius = _radius;
					RaisePropertyChanged();
					RaisePropertyChanged(nameof(SearchParams));
				}
			}
		}

		public int MinPrice
		{
			get => _minPrice;
			set => Set(ref _minPrice, value);
		}

		public int MaxPrice
		{
			get => _maxPrice;
			set => Set(ref _maxPrice, value);
		}

		public int MinBedrooms
		{
			get => _minBedrooms;
			set => Set(ref _minBedrooms, value);
		}

		public int MaxBedrooms
		{
			get => _maxBedrooms;
			set => Set(ref _maxBedrooms, value);
		}

		public SortType SortType
		{
			get => _sortType;
			set => Set(ref _sortType, value);
		}

		/// <summary>
		/// Gets or sets the property type
		/// </summary>
		public PropertyTypeEnum PropertyType
		{
			get => _propertyType;
			set => Set(ref _propertyType, value);
		}

		//// Using a DependencyProperty as the backing store for MySelectedItem.  This enables animation, styling, binding, etc...
		//public static readonly DependencyProperty SearchParamsProperty =
		//	DependencyProperty.Register("SearchParams", typeof(SearchParams), typeof(SearchParamsControlViewModel), new UIPropertyMetadata(new SearchParams(), OnSearchParamsChanged));

		//private static void OnSearchParamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//{
		//	throw new NotImplementedException();
		//}
	}
}
