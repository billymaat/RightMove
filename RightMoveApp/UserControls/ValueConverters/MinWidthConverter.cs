using System;
using System.Globalization;
using System.Windows.Data;

namespace RightMove.Desktop.UserControls.ValueConverters
{
	public class MinWidthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? 0 : 200; 
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
