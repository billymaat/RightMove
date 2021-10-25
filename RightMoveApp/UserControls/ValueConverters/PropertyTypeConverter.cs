using RightMove.DataTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace RightMoveApp.UserControls.ValueConverters
{
	public class PropertyTypeConverter : IMultiValueConverter
	{
		private PropertyTypeEnum _target;
		private PropertyTypeEnum _mask;

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			_mask = (PropertyTypeEnum)values[1];
			_target = (PropertyTypeEnum)values[0];

			return (_mask & _target) != 0;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			_target = _target ^ _mask;
			return new object[2] 
				{ _target, _mask } ;
		}
	}
}
