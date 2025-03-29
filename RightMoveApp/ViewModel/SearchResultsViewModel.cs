using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json.Linq;
using RightMove.DataTypes;
using RightMove.Desktop.Helpers;
using RightMove.Desktop.Messages;
using RightMove.Desktop.Model;
using RightMove.Extensions;
using RelayCommand = RightMove.Desktop.ViewModel.Commands.RelayCommand;

namespace RightMove.Desktop.ViewModel
{
	public class SearchResultsViewModel : ObservableObject
	{
		private readonly RightMoveModel _rightMoveModel;
		private readonly IMessenger _messenger;

		public SearchResultsViewModel(RightMoveModel rightMoveModel, IMessenger messenger)
		{
			_rightMoveModel = rightMoveModel;
			_messenger = messenger;

			OpenLink = new RelayCommand(ExecuteOpenLink, CanExecuteOpenLink);

			SelectionChangedCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand<RightMoveProperty>(ExecuteSelectionChanged, (obj) => true);
			InitializeTimers();
		}

		public void SetToken(string token)
		{
			Token = token;
			_messenger.Register<RightMoveSelectedItemUpdatedMessage, string>(this, Token, (recipient, message) => RightMoveSelectedItem = message.NewValue);
			_messenger.Register<RightMoveFullSelectedItemUpdatedMessage, string>(this, Token, (recipient, message) => RightMovePropertyFullSelectedItem = message.NewValue);
			_messenger.Register<RightMovePropertyItemsUpdatedMessage, string>(this, Token, (recipient, message) => RightMovePropertyItems = new ObservableCollection<RightMoveProperty>(message.NewValue));
		}

		public void SetLocation(string text)
		{
			Location = text;
		}
		public string Token { get; set; }

		private RightMoveProperty _rightMoveSelectedItem;
		/// <summary>
		/// Gets or sets the selected <see cref="RightMoveViewItem"/>
		/// </summary>
		public RightMoveProperty RightMoveSelectedItem
		{
			get => _rightMoveSelectedItem;
			set => SetProperty(ref _rightMoveSelectedItem, value);
		}

		public string Location
		{
			get => _location;
			set => SetProperty(ref _location, value);
		}

		public string SubLocation
		{
			get
			{
				return Location?.Split(',').First();
			}
		}

		/// <summary>
		/// Gets or sets the open link command
		/// </summary>
		public ICommand OpenLink
		{
			get;
			set;
		}

		private string _info;

		/// <summary>
		/// Gets or sets the Info
		/// </summary>
		public string Info
		{
			get => _info;
			set => SetProperty(ref _info, value);
		}

		private ObservableCollection<RightMoveProperty> _rightMovePropertyItems;

		/// <summary>
		/// Gets or sets the right move items
		/// </summary>
		public ObservableCollection<RightMoveProperty> RightMovePropertyItems
		{
			get => _rightMovePropertyItems;
			set => SetProperty(ref _rightMovePropertyItems, value);
		}

		private RightMoveProperty _rightMovePropertyFullSelectedItem;

		public RightMoveProperty RightMovePropertyFullSelectedItem
		{
			get => _rightMovePropertyFullSelectedItem;
			set => SetProperty(ref _rightMovePropertyFullSelectedItem, value);
		}

		/// <summary>
		/// Execute open link command
		/// </summary>
		/// <param name="obj">the object</param>
		private void ExecuteOpenLink(object obj)
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
		/// <param name="arg">the argument</param>
		/// <returns>true if can execute, false otherwise</returns>
		private bool CanExecuteOpenLink(object arg)
		{
			return true;
		}

		private void UpdateAveragePrice()
		{
			if (RightMovePropertyItems != null)
			{
				StringBuilder sb = new StringBuilder();

				var averagePrice = RightMovePropertyItems.AveragePrice();
				if (averagePrice != double.MinValue)
				{
					sb.AppendLine($"Average price: {averagePrice.ToString("C2")}");
				}
				sb.Append($"Property count: {RightMovePropertyItems.Count}");
				Info = sb.ToString();
			}
			else
			{
				Info = "...";
			}
		}

		private async void SelectedItemChanged_Elapsed(object sender, EventArgs e)
		{
			_selectedItemChangedTimer.Stop();

			try
			{
				_tokenSource.Cancel();

				_tokenSource = new CancellationTokenSource();
				CancellationToken cancellationToken = _tokenSource.Token;

				//await UpdateFullSelectedItemAndImage(cancellationToken);
			}
			catch (Exception)
			{
				System.Diagnostics.Debug.WriteLine("Operation exception");
			}
		}

		private async Task ExecuteSelectionChanged(RightMoveProperty rightMoveProperty)
		{
			if (rightMoveProperty == null)
			{
				return;
			}

			// need to parse the full image
			await _rightMoveModel.UpdateSelectedRightMoveItem(rightMoveProperty.RightMoveId, _tokenSource.Token);
		}

		// cancellation token
		private CancellationTokenSource _tokenSource = new CancellationTokenSource();

		private void InitializeTimers()
		{
			_selectedItemChangedTimer = new System.Windows.Threading.DispatcherTimer();
			_selectedItemChangedTimer.Interval = TimeSpan.FromMilliseconds(500);
			_selectedItemChangedTimer.Tick += SelectedItemChanged_Elapsed;
		}

		public ICommand SelectionChangedCommand
		{
			get => _selectionChangedCommand;
			set => SetProperty(ref _selectionChangedCommand, value);
		}

		// Time for selected item changed in data grid
		private System.Windows.Threading.DispatcherTimer _selectedItemChangedTimer;

		private ICommand _selectionChangedCommand;
		private string _location;
	}
}
