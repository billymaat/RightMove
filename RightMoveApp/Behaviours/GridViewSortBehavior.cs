using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RightMove.Desktop.Behaviours
{
    public static class GridViewSortBehavior
    {
        public static readonly DependencyProperty EnableSortingProperty =
            DependencyProperty.RegisterAttached(
                "EnableSorting",
                typeof(bool),
                typeof(GridViewSortBehavior),
                new PropertyMetadata(false, OnEnableSortingChanged));

        public static bool GetEnableSorting(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableSortingProperty);
        }

        public static void SetEnableSorting(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableSortingProperty, value);
        }

        private static void OnEnableSortingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListView listView)
            {
                if ((bool)e.NewValue)
                {
                    listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(OnColumnHeaderClick));
                }
                else
                {
                    listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(OnColumnHeaderClick));
                }
            }
        }

        private static void OnColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader headerClicked && headerClicked.Role != GridViewColumnHeaderRole.Padding)
            {
                var listView = sender as ListView;
                if (listView == null)
                    return;

                var propertyName = headerClicked.Tag as string ?? headerClicked.Content as string;
                if (string.IsNullOrEmpty(propertyName))
                    return;

                var view = CollectionViewSource.GetDefaultView(listView.ItemsSource);
                if (view == null)
                    return;

                var direction = ListSortDirection.Ascending;

                if (view.SortDescriptions.Count > 0)
                {
                    var currentSort = view.SortDescriptions[0];
                    if (currentSort.PropertyName == propertyName)
                    {
                        direction = currentSort.Direction == ListSortDirection.Ascending
                            ? ListSortDirection.Descending
                            : ListSortDirection.Ascending;
                    }
                }

                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
                view.Refresh();
            }
        }
    }
}
