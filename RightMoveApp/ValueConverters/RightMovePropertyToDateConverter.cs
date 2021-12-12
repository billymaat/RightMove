﻿using RightMove.DataTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace RightMoveApp.ValueConverters
{
	public class RightMovePropertyToDateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var rightMoveProperty = value as RightMoveProperty;
			if (rightMoveProperty is null)
			{
				return null;
			}

			if (!rightMoveProperty.DateAdded.Equals(DateTime.MinValue))
			{
				return rightMoveProperty.DateAdded.ToString("dd-MM-yyyy");
			}

			if (!rightMoveProperty.DateReduced.Equals(DateTime.MinValue))
			{
				var dateString = rightMoveProperty.DateReduced.ToString("dd-MM-yyyy");
				return $"{dateString} (r)";
			}

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
