using System;
using System.Globalization;
using System.Windows.Data;

namespace RightMove.Desktop.ValueConverters
{
	public class NullToNAConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return "NA";
			}

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
