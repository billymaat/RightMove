using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RightMove.Desktop.Behaviours
{
	public static class ListViewItemDoubleClickBehavior
	{
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.RegisterAttached(
				"Command",
				typeof(ICommand),
				typeof(ListViewItemDoubleClickBehavior),
				new PropertyMetadata(null, OnChanged));

		public static void SetCommand(DependencyObject d, ICommand value)
			=> d.SetValue(CommandProperty, value);

		public static ICommand GetCommand(DependencyObject d)
			=> (ICommand)d.GetValue(CommandProperty);

		private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ListView listView)
			{
				if (e.OldValue != null)
					listView.PreviewMouseDoubleClick -= OnDoubleClick;

				if (e.NewValue != null)
					listView.PreviewMouseDoubleClick += OnDoubleClick;
			}
		}

		private static void OnDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var listView = (ListView)sender;
			var command = GetCommand(listView);
			if (command == null)
				return;

			DependencyObject source = (DependencyObject)e.OriginalSource;

			while (source != null)
			{
				if (source is GridViewColumnHeader)
					return;

				if (source is ListViewItem item)
				{
					var parameter = item.DataContext;
					if (command.CanExecute(parameter))
						command.Execute(parameter);
					return;
				}

				source = VisualTreeHelper.GetParent(source);
			}
		}
	}
}
