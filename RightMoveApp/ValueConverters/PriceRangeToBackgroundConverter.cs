using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RightMove.Desktop.ValueConverters
{
	public class PriceRangeToBackgroundConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values.Length < 2)
				return Brushes.Transparent;

			// values[0] is the nearby sold property price (double?)
			// values[1] is the current property price (int)

			if (values[0] is double nearbySoldPrice && values[1] is int currentPrice && currentPrice > 0)
			{
				double lowerBound = currentPrice * 0.9;  // 10% below
				double upperBound = currentPrice * 1.1;  // 10% above

				if (nearbySoldPrice >= lowerBound && nearbySoldPrice <= upperBound)
				{
					return new SolidColorBrush(Color.FromRgb(144, 238, 144)); // Light green
				}
			}

			return Brushes.Transparent;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
