using RightMove.DataTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace RightMoveApp.UserControls.ValueConverters
{
	public class PropertyTypeConverter : IValueConverter
	{
		private PropertyTypeEnum _target;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			PropertyTypeEnum mask = (PropertyTypeEnum)parameter;
			_target = (PropertyTypeEnum)value;

			return (mask & _target) != 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			_target ^= (PropertyTypeEnum)parameter;
			return _target;
		}
	}
}
