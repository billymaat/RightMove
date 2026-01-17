using System;
using System.Globalization;
using System.Windows.Data;

namespace RightMove.Desktop.ValueConverters
{
	public class NegativeOneToNAConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is int intValue && intValue == -1)
			{
				return "NA";
			}

			return value?.ToString() ?? "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
