﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace RightMove.Desktop.UserControls
{
	public class PriceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double price = (double)value;

			return price.ToString("C2");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
