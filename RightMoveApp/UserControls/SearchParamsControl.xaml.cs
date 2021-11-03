using System;
using System.Collections.Generic;
using System.Linq;
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
	public partial class SearchParamsControl
	{
		public SearchParamsControl()
		{
			InitializeComponent();

			LayoutRoot.DataContext = this;
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

		public PropertyTypeEnum PropType
		{
			get;
			set;
		}

		public StringTrieSet SearchString
		{
			get
			{
				return RightMoveCodes.RegionTree;
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
			DependencyProperty.Register("SearchParams", typeof(SearchParams), typeof(SearchParamsControl), new UIPropertyMetadata(new SearchParams()));
	}
}
