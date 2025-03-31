using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json.Linq;
using RightMove.DataTypes;
using RightMove.Desktop.Helpers;
using RightMove.Desktop.Messages;
using RightMove.Desktop.Model;
using RightMove.Extensions;
using ServiceCollectionUtilities;
using RelayCommand = RightMove.Desktop.ViewModel.Commands.RelayCommand;

namespace RightMove.Desktop.ViewModel
{
	public class SearchResultsViewModel : ObservableObject
	{
		private RightMoveModel _rightMoveModel;
		private readonly IFactory<PropertyInfoViewModel> _propertyInfoViewModelFactory;
		private readonly IMessenger _messenger;

		public SearchResultsViewModel(IFactory<PropertyInfoViewModel> propertyInfoViewModelFactory,
			IMessenger messenger)
		{
			_propertyInfoViewModelFactory = propertyInfoViewModelFactory;
			_messenger = messenger;

			OpenLink = new RelayCommand(ExecuteOpenLink, CanExecuteOpenLink);

			SelectionChangedCommand = new CommunityToolkit.Mvvm.Input.AsyncRelayCommand<RightMoveProperty>(ExecuteSelectionChanged, (obj) => true);
			InitializeTimers();

			KeyDownCommand = new RelayCommand<KeyEventArgs>(ExecuteKeyDown);
		}

		public void SetRightMoveModel(RightMoveModel rightMoveModel)
		{
			_rightMoveModel = rightMoveModel;
		}

		public void SetToken(string token)
		{
			Token = token;
			_messenger.Register<RightMoveSelectedItemUpdatedMessage, string>(this, Token, (recipient, message) => RightMoveSelectedItem = message.NewValue);
			_messenger.Register<RightMoveFullSelectedItemUpdatedMessage, string>(this, Token, (recipient, message) => RightMovePropertyFullSelectedItem = message.NewValue);
			_messenger.Register<RightMovePropertyItemsUpdatedMessage, string>(this, Token, (recipient, message) => RightMovePropertyItems = new ObservableCollection<RightMoveProperty>(message.NewValue));
			//_messenger.Register<NextImageMessage, string>(this, Token, (recipient, message) => NextImage());
			//_messenger.Register<PrevImageMessage, string>(this, Token, (recipient, message) => PrevImage());
		}

		private void PrevImage()
		{
			_messenger.Send(new PrevImageMessage(), Token);
		}

		private void NextImage()
		{
			_messenger.Send(new NextImageMessage(), Token);
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

		public PropertyInfoViewModel PropertyInfoViewModel
		{
			get => _propertyInfoViewModel;
			set => SetProperty(ref _propertyInfoViewModel, value);
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
			set
			{
				if (SetProperty(ref _rightMovePropertyFullSelectedItem, value))
				{
					PropertyInfoViewModel = _propertyInfoViewModelFactory.Create();
					_propertyInfoViewModel.SetRightMoveModel(_rightMoveModel);
					_propertyInfoViewModel.SetRightMoveProperty(value);
					_propertyInfoViewModel.SetToken(Token);
				}
			}
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

		public int ImageIndexView
		{
			get => _imageIndexView;
			set => SetProperty(ref _imageIndexView, value);
		}

		// Time for selected item changed in data grid
		private System.Windows.Threading.DispatcherTimer _selectedItemChangedTimer;

		private ICommand _selectionChangedCommand;
		private string _location;
		private int _imageIndexView;
		private PropertyInfoViewModel _propertyInfoViewModel;

		public ICommand KeyDownCommand
		{
			get;
		}


		private void ExecuteKeyDown(KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.A:
					_messenger.Send(new PrevImageMessage(), Token);
					break;
				case Key.S:
					_messenger.Send(new NextImageMessage(), Token);
					break;
			}
		}
	}
}
