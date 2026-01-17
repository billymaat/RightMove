using System;
using System.Globalization;
using System.Windows.Data;
using RightMove.DataTypes;

namespace RightMove.Desktop.ValueConverters
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

			if (!rightMoveProperty.DateUpdated.Equals(DateTime.MinValue))
			{
				var dateString = rightMoveProperty.DateUpdated.ToString("dd-MM-yyyy");
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
