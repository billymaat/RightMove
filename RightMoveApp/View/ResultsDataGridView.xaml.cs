using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RightMove.DataTypes;

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

            Delay = 3000;
        }

        public static readonly DependencyProperty RightMoveSelectedItemProperty = DependencyProperty.Register(
            nameof(RightMoveSelectedItem), typeof(RightMoveProperty), typeof(ResultsDataGridView), new PropertyMetadata(default(RightMoveProperty)));

        public RightMoveProperty RightMoveSelectedItem
        {
            get { return (RightMoveProperty)GetValue(RightMoveSelectedItemProperty); }
            set { SetValue(RightMoveSelectedItemProperty, value); }
        }

        public static readonly DependencyProperty RightMovePropertyItemsProperty = DependencyProperty.Register(
            nameof(RightMovePropertyItems), typeof(IEnumerable<RightMoveProperty>), typeof(ResultsDataGridView), new PropertyMetadata(default(IEnumerable<RightMoveProperty>)));

        public IEnumerable<RightMoveProperty> RightMovePropertyItems
        {
            get { return (IEnumerable<RightMoveProperty>)GetValue(RightMovePropertyItemsProperty); }
            set { SetValue(RightMovePropertyItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectionChangedProperty = DependencyProperty.Register(
            nameof(SelectionChanged), typeof(ICommand), typeof(ResultsDataGridView), new PropertyMetadata(default(ICommand)));

        public ICommand SelectionChanged
        {
            get { return (ICommand)GetValue(SelectionChangedProperty); }
            set { SetValue(SelectionChangedProperty, value); }
        }

        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(
            nameof(Delay), typeof(int), typeof(ResultsDataGridView), new PropertyMetadata(default(int)));

        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        public static readonly DependencyProperty OpenLinkProperty = DependencyProperty.Register(
	        nameof(OpenLink), typeof(ICommand), typeof(ResultsDataGridView), new PropertyMetadata(default(ICommand)));

        public ICommand OpenLink
        {
	        get { return (ICommand)GetValue(OpenLinkProperty); }
	        set { SetValue(OpenLinkProperty, value); }
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
