using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RightMove.Desktop.View
{
	/// <summary>
	/// Interaction logic for ResultsDataGridView.xaml
	/// </summary>
	public partial class ResultsDataGridView : UserControl
	{
		private GridViewColumnHeader _lastHeaderClicked;
		private ListSortDirection _lastDirection = ListSortDirection.Ascending;

		public ResultsDataGridView()
		{
			InitializeComponent();
		}


		/// <summary>
		/// Grid view column event handler clicked
		/// </summary>
		/// <param name="sender">the sender</param>
		/// <param name="e">the event args</param>
		private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
		{
			if (!(e.OriginalSource is GridViewColumnHeader gridViewColumnHeader)) return;
			var dir = ListSortDirection.Ascending;
			if (gridViewColumnHeader == _lastHeaderClicked && _lastDirection == ListSortDirection.Ascending)
			{
				dir = ListSortDirection.Descending;
			}

			Sort(gridViewColumnHeader, dir);
			_lastHeaderClicked = gridViewColumnHeader;
			_lastDirection = dir;
		}

		/// <summary>
		/// Sort by column header
		/// </summary>
		/// <param name="ch">the column header</param>
		/// <param name="dir">the sort direction</param>
		private void Sort(GridViewColumnHeader ch, ListSortDirection dir)
		{
			var bindingPath = (ch.Column.DisplayMemberBinding as Binding)?.Path.Path;
			bindingPath ??= ch.Column.Header as string;
			var collectionView = CollectionViewSource.GetDefaultView(listView.ItemsSource);
			if (collectionView is null)
			{
				return;
			}

			collectionView.SortDescriptions.Clear();
			var sd = new SortDescription(bindingPath, dir);
			collectionView.SortDescriptions.Add(sd);
			collectionView.Refresh();
		}
	}
}
