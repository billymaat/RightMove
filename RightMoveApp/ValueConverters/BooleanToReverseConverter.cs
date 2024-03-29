﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace RightMoveApp.ValueConverters
{
	public class BooleanToReverseConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		 => !(bool?)value ?? true;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		 => !(value as bool?);
	}
}
