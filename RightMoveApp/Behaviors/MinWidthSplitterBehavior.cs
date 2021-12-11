using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace RightMoveApp.Behaviors
{
    public class MinWidthSplitterBehavior : Behavior<Grid>
    {
        public Grid ParentGrid { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            ParentGrid = this.AssociatedObject as Grid;
            ParentGrid.SizeChanged += Parent_SizeChanged;            
        }

        private void Parent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ParentGrid.ColumnDefinitions.Count == 3)
            {
                Double maxW = e.NewSize.Width - ParentGrid.ColumnDefinitions[2].MinWidth -
                              ParentGrid.ColumnDefinitions[1].ActualWidth;
                ParentGrid.ColumnDefinitions[0].MaxWidth = maxW;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (ParentGrid != null)
            {
                ParentGrid.SizeChanged -= Parent_SizeChanged;
            }
        }
    }
}
