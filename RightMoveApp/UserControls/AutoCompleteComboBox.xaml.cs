using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RightMove.DataTypes;

namespace RightMove.Desktop.UserControls
{
	/// <summary>
	/// Interaction logic for AutoCompleteComboBox.xaml
	/// </summary>
	public partial class AutoCompleteComboBox : UserControl
	{
		public ListCollectionView LstCollectionView
		{
			get;
			set;
		}

		public AutoCompleteComboBox()
		{
			InitializeComponent();

			// Attach events to the controls
			txtAuto.TextChanged += TxtAuto_TextChanged;
			txtAuto.PreviewKeyDown += TxtAuto_PreviewKeyDown;
            lstSuggestion.SelectionChanged += ListBox_SelectionChanged;
        }

		#region Properties

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteComboBox), new PropertyMetadata(default(string)));

		public List<string> ItemsSource
		{
			get { return (List<string>)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(List<string>), typeof(AutoCompleteComboBox), new UIPropertyMetadata(null, OnItemsSourceChanged));

		private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			AutoCompleteComboBox ctrl = d as AutoCompleteComboBox;
			if (ctrl != null)
			{
				ctrl.lstSuggestion.ItemsSource = (List<string>)e.NewValue;
			}
		}

		/// 
		/// Gets or sets the selected value.
		/// 
		/// The selected value.
		public RightMoveRegion SelectedValue
		{
			get { return (RightMoveRegion)GetValue(SelectedValueProperty); }
			set { SetValue(SelectedValueProperty, value); }
		}

		public RightMoveRegion SelectedRightMoveRegion
		{
			get { return (RightMoveRegion)GetValue(SelectedRightMoveRegionProperty); }
			set { SetValue(SelectedRightMoveRegionProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedRightMoveRegion.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedRightMoveRegionProperty =
			DependencyProperty.Register("SelectedRightMoveRegion", typeof(RightMoveRegion), typeof(AutoCompleteComboBox), new FrameworkPropertyMetadata(default(RightMoveRegion), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        private const int DefaultMinimumCharactersForSearch = 3;


        public int MinimumCharactersForSearch
		{
			get { return (int)GetValue(MinimumCharactersForSearchProperty); }
			set { SetValue(MinimumCharactersForSearchProperty, value); }
		}

		// Using a DependencyProperty as the backing store for MinimumCharactersForSearch.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MinimumCharactersForSearchProperty =
			DependencyProperty.Register("MinimumCharactersForSearch", typeof(int), typeof(AutoCompleteComboBox), new PropertyMetadata(DefaultMinimumCharactersForSearch));


		// Using a DependencyProperty as the backing store for SelectedValue.  
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedValueProperty =
			DependencyProperty.Register("SelectedValue"
							, typeof(RightMoveRegion)
							, typeof(AutoCompleteComboBox)
							, new UIPropertyMetadata(default(RightMoveRegion)));

        private CancellationTokenSource _tokenSource = new();

		/// 
		/// Handles the TextChanged event of the autoTextBox control.
		/// 
		/// <param name="sender">The source of the event.
		/// <param name="e">The instance containing the event data.
		private async void TxtAuto_TextChanged(object sender, TextChangedEventArgs e)
		{
			await _tokenSource.CancelAsync();
            _tokenSource = new CancellationTokenSource();
			var token = _tokenSource.Token;

            // Only autocomplete when there is text
            if (txtAuto.Text.Length == 0)
			{
				lstSuggestion.Visibility = Visibility.Collapsed;
				return;
			}

            if (string.IsNullOrEmpty(txtAuto.Text) || TextBoxIsAboveMinimumCharacters())
            {
                return;
            }

            var regionService = new RightMoveRegionService();

            try
            {
                var items = (await regionService.SearchAsync(txtAuto.Text, token)).ToList();
                lstSuggestion.ItemsSource = items;
                lstSuggestion.Visibility = (items.Count == 0)
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
            catch (TaskCanceledException)
            {
            }
		}

        private bool TextBoxIsAboveMinimumCharacters()
        {
            return txtAuto.Text.Length < MinimumCharactersForSearch;
        }

        /// 
		/// Handles the PreviewKeyDown event of the autoTextBox control.
		/// 
		/// <param name="sender">The source of the event.
		/// <param name="e">The instance containing the event data.
		private void TxtAuto_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Down:
					if (lstSuggestion.SelectedIndex < lstSuggestion.Items.Count)
					{
						lstSuggestion.SelectedIndex = lstSuggestion.SelectedIndex + 1;
					}
					break;
				case Key.Up:
					if (lstSuggestion.SelectedIndex > -1)
					{
						lstSuggestion.SelectedIndex = lstSuggestion.SelectedIndex - 1;
					}
					break;
				case Key.Enter:
				case Key.Tab:
					// Commit the selection
					lstSuggestion.Visibility = Visibility.Collapsed;
					e.Handled = (e.Key == Key.Enter);
					break;
				case Key.Escape:
					// Cancel the selection
					//lstSuggestion.ItemsSource = null;
					lstSuggestion.Visibility = Visibility.Collapsed;
					break;
				default:
					break;
			}
		}

		/// 
		/// Handles the SelectionChanged event of the suggestionListBox control.
		/// 
		/// <param name="sender">The source of the event.
		/// <param name="e">The instance containing the event data.
		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            if (lstSuggestion.ItemsSource != null)
            {
                txtAuto.TextChanged -= TxtAuto_TextChanged;
                if (lstSuggestion.SelectedIndex != -1)
                {
                    if (lstSuggestion.SelectedItem is RightMoveRegion region)
                    {
                        UpdateTextBox(region);
                    }
                }
                txtAuto.TextChanged += TxtAuto_TextChanged;
            }
        }

        private void UpdateTextBox(RightMoveRegion region)
        {
            Text = region.DisplayName;
        }


        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			// Cancel the selection
			//lstSuggestion.ItemsSource = null;
			lstSuggestion.Visibility = Visibility.Collapsed;
		}

		#endregion
	}
}
