using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace RightMove.Desktop.Behaviours
{
	public static class DataGridRowDoubleClickBehavior
	{
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.RegisterAttached(
				"Command",
				typeof(ICommand),
				typeof(DataGridRowDoubleClickBehavior),
				new PropertyMetadata(null, OnChanged));

		public static void SetCommand(DependencyObject d, ICommand value)
			=> d.SetValue(CommandProperty, value);

		public static ICommand GetCommand(DependencyObject d)
			=> (ICommand)d.GetValue(CommandProperty);

		private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is DataGrid grid)
			{
				if (e.OldValue != null)
					grid.PreviewMouseDoubleClick -= OnDoubleClick;

				if (e.NewValue != null)
					grid.PreviewMouseDoubleClick += OnDoubleClick;
			}
		}

		private static void OnDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var grid = (DataGrid)sender;
			var command = GetCommand(grid);
			if (command == null)
				return;

			DependencyObject source = (DependencyObject)e.OriginalSource;

			while (source != null)
			{
				// Ignore column headers & resize thumbs
				if (source is DataGridColumnHeader || source is Thumb)
					return;

				// Execute only on DataGridRow
				if (source is DataGridRow row)
				{
					var parameter = row.Item;
					if (command.CanExecute(parameter))
						command.Execute(parameter);
					return;
				}

				source = VisualTreeHelper.GetParent(source);
			}
		}
	}
}
