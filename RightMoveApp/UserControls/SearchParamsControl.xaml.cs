using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RightMove;
using RightMove.DataTypes;
using Utilities;
using static RightMove.DataTypes.SearchParams;

namespace RightMoveApp.UserControls
{
	/// <summary>
	/// Interaction logic for SearchParamsControl.xaml
	/// </summary>
	public partial class SearchParamsControl : INotifyPropertyChanged
	{
		public event EventHandler SearchParamsUpdated;

		public SearchParamsControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Radius entries bound to combobox
		/// </summary>
		public Dictionary<double, string> RadiusEntries
		{
			get;
			set;
		} = new Dictionary<double, string>()
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

		public Dictionary<PropertyTypeEnum, string> PropertyTypes => PropertyTypeDictionary;

		/// <summary>
		/// Prices bound to combo box
		/// </summary>
		public List<int> Prices
		{
			get;
			set;
		} = new List<int>()
		{
			0,
			50000,
			60000,
			70000,
			80000,
			90000,
			100000,
			110000,
			120000,
			125000,
			130000,
			150000,
			200000,
			250000,
			300000,
			325000,
			375000,
			400000,
			425000,
			450000,
			475000,
			500000,
			550000,
			600000,
			650000,
			700000,
			800000,
			900000,
			1000000,
			1250000,
			1500000,
			1750000,
			2000000,
			2500000,
			3000000,
			4000000,
			5000000,
			7500000,
			10000000,
			15000000,
			20000000
		};

		/// <summary>
		/// Bedrooms bound to combobox
		/// </summary>
		public List<int> Bedrooms
		{
			get;
			set;
		} = new List<int>()
		{
			0,
			1,
			2,
			3,
			4,
			5
		};

		public List<string> SearchString
		{
			get
			{
				return RightMoveCodes.RegionTree;
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

		public SearchParams SearchParams
		{
			get
			{
				SearchParams searchParams = (SearchParams)GetValue(SearchParamsProperty);
				return searchParams;
			}
			set
			{
				SetValue(SearchParamsProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for MySelectedItem.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SearchParamsProperty =
			DependencyProperty.Register("SearchParams", typeof(SearchParams), typeof(SearchParamsControl), new PropertyMetadata(new SearchParams(), OnSearchParamsPropertyChanged));

		public event PropertyChangedEventHandler PropertyChanged;

		private static void OnSearchParamsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//SearchParamsControl c = d as SearchParamsControl;

			//if (c != null)
			//{
			//	c.OnSearchParamsChanged();
			//}
		}

		private void OnSearchParamsChanged()
		{
			SearchParamsUpdated?.Invoke(this, new EventArgs());
			OnPropertyChanged(nameof(SearchParams));
		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
