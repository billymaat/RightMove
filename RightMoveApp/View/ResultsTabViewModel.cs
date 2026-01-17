using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RightMove.DataTypes;
using RightMove.Desktop.Helpers;
using RightMove.Desktop.Model;
using RightMove.Desktop.ViewModel;
using RightMove.Extensions;

namespace RightMove.Desktop.View
{
	public class ResultsTabViewModel : ObservableRecipient
	{
		private readonly RightMoveModelService _rightMoveModelService;

		private ObservableCollection<RightMoveProperty> _rightMovePropertyItems;
		// cancellation token
		private CancellationTokenSource _tokenSource = new CancellationTokenSource();
		// Time for selected item changed in data grid
		private System.Windows.Threading.DispatcherTimer _selectedItemChangedTimer;

		private RightMoveProperty _rightMoveSelectedItem;
		private string _title;
		private string _info;

		/// <summary>
		/// Gets or sets the selected <see cref="RightMoveViewItem"/>
		/// </summary>
		public RightMoveProperty RightMoveSelectedItem
		{
			get => _rightMoveSelectedItem;
			set => SetProperty(ref _rightMoveSelectedItem, value);
		}

		public PropertyInfoViewModel PropertyInfoViewModel { get; }

		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		public string Info

		{
			get => _info;
			set => SetProperty(ref _info, value);
		}

		public ResultsTabViewModel(PropertyInfoViewModel propertyInfoViewModel, RightMoveModel rightMoveModel, RightMoveModelService rightMoveModelService)
		{
			_rightMoveModelService = rightMoveModelService;
			PropertyInfoViewModel = propertyInfoViewModel;

			SelectionChangedCommand = new RelayCommand<RightMoveProperty>(ExecuteSelectionChanged, (obj) => true);
			OpenLink = new RelayCommand(ExecuteOpenLink, CanExecuteOpenLink);

			RightMovePropertyItems = new ObservableCollection<RightMoveProperty>(rightMoveModel.RightMovePropertyItems);
			//rightMoveModel.RightMovePropertyItemsUpdated += (s, e) =>
			//{
			//	RightMovePropertyItems = new ObservableCollection<RightMoveProperty>(e.NewValue);
			//};

			UpdateAveragePrice();
			InitializeTimers();
		}

		/// <summary>
		/// Gets or sets the right move items
		/// </summary>
		public ObservableCollection<RightMoveProperty> RightMovePropertyItems
		{
			get => _rightMovePropertyItems;
			set => SetProperty(ref _rightMovePropertyItems, value);
		}

		public ICommand SelectionChangedCommand
		{
			get;
		}

		/// <summary>
		/// Gets or sets the open link command
		/// </summary>
		public ICommand OpenLink
		{
			get;
		}

		private void ExecuteSelectionChanged(RightMoveProperty rightMoveProperty)
		{
			if (rightMoveProperty == null)
			{
				return;
			}

			// Debounce: stop the current timer and restart it
			// This ensures we only process the selection after the user stops scrolling
			_selectedItemChangedTimer.Stop();

			// Store the selected item temporarily
			RightMoveSelectedItem = rightMoveProperty;

			// Start the timer - the actual processing happens in SelectedItemChanged_Elapsed
			_selectedItemChangedTimer.Start();
		}

		private async void SelectedItemChanged_Elapsed(object sender, EventArgs e)
		{
			_selectedItemChangedTimer.Stop();

			try
			{
				_tokenSource.Cancel();

				_tokenSource = new CancellationTokenSource();
				CancellationToken cancellationToken = _tokenSource.Token;

				// Process the model update with the debounced selected item
				_rightMoveModelService.UpdateSelectedRightMoveItem(RightMoveSelectedItem.RightMoveId, cancellationToken);
			}
			catch (Exception)
			{
				System.Diagnostics.Debug.WriteLine("Operation exception");
			}
		}

		private void InitializeTimers()
		{
			_selectedItemChangedTimer = new System.Windows.Threading.DispatcherTimer();
			_selectedItemChangedTimer.Interval = TimeSpan.FromMilliseconds(500);
			_selectedItemChangedTimer.Tick += SelectedItemChanged_Elapsed;
		}

		/// <summary>
		/// Execute open link command
		/// </summary>
		private void ExecuteOpenLink()
		{
			if (RightMoveSelectedItem is null)
			{
				return;
			}

			BrowserHelper.OpenWebpage(RightMoveSelectedItem.Url);
		}

		/// <summary>
		/// Can execute open link command
		/// </summary>
		/// <returns>true if can execute, false otherwise</returns>
		private bool CanExecuteOpenLink()
		{
			return true;
		}

		private void UpdateAveragePrice()
		{
			if (RightMovePropertyItems != null)
			{
				StringBuilder sb = new StringBuilder();

				var lst = new List<string>();
				var averagePrice = RightMovePropertyItems.AveragePrice();
				if (averagePrice != double.MinValue)
				{

					lst.Add($"Average price: {averagePrice.ToString("C2")}");
				}
				lst.Add($"Property count: {RightMovePropertyItems.Count}");
				Info = string.Join(", ", lst);
			}
			else
			{
				Info = "...";
			}
		}
	}
}
