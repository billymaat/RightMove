using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using System.ComponentModel;

namespace RightMoveApp.UserControls
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
			txtAuto.TextChanged +=
				new TextChangedEventHandler(TxtAuto_TextChanged);
			txtAuto.PreviewKeyDown +=
				new KeyEventHandler(TxtAuto_PreviewKeyDown);

			lstSuggestion.SelectionChanged +=
				new SelectionChangedEventHandler(ListBox_SelectionChanged);

			lstSuggestion.ItemsSource = ItemsSource;
		}

		#region Properties



		public List<string> ItemsSource
		{
			get { return (List<string>)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(List<string>), typeof(AutoCompleteComboBox), new UIPropertyMetadata(null));

		///// 
		///// Gets or sets the items source.
		///// 
		///// The items source.
		//public StringTrieSet ItemsSource
		//{
		//	get { return (StringTrieSet)GetValue(ItemsSourceProperty); }
		//	set { SetValue(ItemsSourceProperty, value); }
		//}

		//// Using a DependencyProperty as the backing store for ItemsSource.  
		//// This enables animation, styling, binding, etc...
		//public static readonly DependencyProperty ItemsSourceProperty =
		//	DependencyProperty.Register("ItemsSource"
		//						, typeof(StringTrieSet)
		//						, typeof(AutoCompleteComboBox)
		//						, new UIPropertyMetadata(null));

		/// 
		/// Gets or sets the selected value.
		/// 
		/// The selected value.
		public string SelectedValue
		{
			get { return (string)GetValue(SelectedValueProperty); }
			set { SetValue(SelectedValueProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedValue.  
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedValueProperty =
			DependencyProperty.Register("SelectedValue"
							, typeof(string)
							, typeof(AutoCompleteComboBox)
							, new UIPropertyMetadata(string.Empty));

		/// 
		/// Handles the TextChanged event of the autoTextBox control.
		/// 
		/// <param name="sender">The source of the event.
		/// <param name="e">The instance containing the event data.
		private async void TxtAuto_TextChanged(object sender, TextChangedEventArgs e)
		{
			// Only autocomplete when there is text
			if (txtAuto.Text.Length == 0)
			{
				lstSuggestion.Visibility = Visibility.Collapsed;
				//lstSuggestion.ItemsSource = null;
				return;
			}

			lstSuggestion.ItemsSource = ItemsSource;

			// Use Linq to Query ItemsSource for resultdata
			string condition = string.Format("{0}%", txtAuto.Text);

			// IEnumerable<string> results = ItemsSource.Where(o => o.ToLower().StartsWith(txtAuto.Text.ToLower()));

			var itemsSource = ItemsSource;
			var text = txtAuto.Text;
			//IEnumerable<string> results = await Task.Run(() => itemsSource.GetByPrefix(text), );
			//var task = Task.Run(() => itemsSource.GetByPrefix(text));
			//await task;
			ICollectionView view = CollectionViewSource.GetDefaultView(itemsSource);
			view.Filter = (o) => { return ((string)o).ToLower().StartsWith(text); };
			// var results = task.Result;

			if (!view.Cast<string>().Any())
			{
				lstSuggestion.Visibility = Visibility.Collapsed;
				//lstSuggestion.ItemsSource = null;
				return;
			}

			// this is where the filtering needs to happen
			//lstSuggestion.ItemsSource = results;
			lstSuggestion.Visibility = Visibility.Visible;
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
			UpdateTextBox();
		}

		private void UpdateTextBox()
		{
			if (lstSuggestion.ItemsSource != null)
			{
				txtAuto.TextChanged
					-= new TextChangedEventHandler(TxtAuto_TextChanged);
				if (lstSuggestion.SelectedIndex != -1)
				{
					txtAuto.Text = lstSuggestion.SelectedItem.ToString();
				}
				txtAuto.TextChanged
					+= new TextChangedEventHandler(TxtAuto_TextChanged);
			}
		}

		private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			// Cancel the selection
			lstSuggestion.ItemsSource = null;
			lstSuggestion.Visibility = Visibility.Collapsed;
		}

		#endregion
	}
}
