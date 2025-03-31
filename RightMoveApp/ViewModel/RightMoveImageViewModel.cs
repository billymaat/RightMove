using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RightMove.DataTypes;
using RightMove.Desktop.Messages;
using RightMove.Desktop.Services;

namespace RightMove.Desktop.ViewModel
{
	public class RightMoveImageViewModel : ObservableRecipient
	{
		private int _imgIndex;

		private readonly RightMoveImageService _rightMoveImageService;
		private readonly IMessenger _messenger;
		private RightMoveProperty _rightMoveProperty;
		private string _imageIndexView;
		private BitmapImage _image;
		private bool _loadingImage;
		private bool _nextButtonEnabled;
		private bool _prevButtonEnabled;

		public RightMoveImageViewModel(RightMoveImageService rightMoveImageService, IMessenger messenger)
		{
			_rightMoveImageService = rightMoveImageService;
			_messenger = messenger;
		}

		public ICommand NextImageCommand => new AsyncRelayCommand(async() => await LoadNextImage());
		public ICommand PrevImageCommand => new AsyncRelayCommand(async () => await LoadPrevImage());

		public RightMoveProperty RightMoveProperty
		{
			get => _rightMoveProperty;
			set
			{
				if (SetProperty(ref _rightMoveProperty, value))
				{
					OnRightMovePropertyChanged(value);
				}
			}
		}

		private int ImgIndex
		{
			get => _imgIndex;
			set
			{
				_imgIndex = value;
				OnImgIndexUpdated();
			}
		}

		private void OnImgIndexUpdated()
		{
			ImageIndexView = $"{ImgIndex + 1} of {RightMoveProperty.ImageUrl.Length}";
		}

		private void OnRightMovePropertyChanged(RightMoveProperty rightMoveProperty)
		{
			ResetImage(rightMoveProperty);
		}

		public string ImageIndexView
		{
			get => _imageIndexView;
			set => SetProperty(ref _imageIndexView, value);
		}

		public BitmapImage Image
		{
			get => _image;
			set => SetProperty(ref _image, value);
		}

		public bool LoadingImage
		{
			get => _loadingImage;
			set
			{
				SetProperty(ref _loadingImage, value);
				UpdateButtonsEnabled();
			}
		}

		public bool NextButtonEnabled
		{
			get => _nextButtonEnabled;
			set => SetProperty(ref _nextButtonEnabled, value);
		}

		public bool PrevButtonEnabled
		{
			get => _prevButtonEnabled;
			set => SetProperty(ref _prevButtonEnabled, value);
		}

		private async Task LoadPrevImage()
		{
			if (ImgIndex > 0)
			{
				ImgIndex--;
				await LoadImage(RightMoveProperty, ImgIndex);
			}
		}

		private async Task LoadNextImage()
		{
			if (ImgIndex < RightMoveProperty.ImageUrl.Length - 1)
			{
				ImgIndex++;
				await LoadImage(RightMoveProperty, ImgIndex);
			}
		}

		private async Task ResetImage(RightMoveProperty rightMoveProperty)
		{
			ImgIndex = 0;
			await LoadImage(rightMoveProperty, ImgIndex);
		}

		private async Task LoadImage(RightMoveProperty rightMoveProperty, int imgIndex)
		{
			LoadingImage = true;
			var img = await _rightMoveImageService.GetImage(rightMoveProperty, imgIndex);
			Image = img;
			LoadingImage = false;
		}

		// Callback when IsLoading, Index, or MaxIndex changes
		private void UpdateButtonsEnabled()
		{
			UpdatePrevEnabled();
			UpdateNextEnabled();
		}

		private void UpdatePrevEnabled()
		{
			// Automatically updates IsNextDisabled
			PrevButtonEnabled = !LoadingImage && ImgIndex > 0;
		}

		private void UpdateNextEnabled()
		{
			// Automatically updates IsNextDisabled
			NextButtonEnabled = !LoadingImage && ImgIndex < RightMoveProperty.ImageUrl.Length - 1;
		}

		public void SetToken(string token)
		{
			Token = token;
			_messenger.Register<PrevImageMessage, string>(this, Token, async (obj, message) => await LoadPrevImage());
			_messenger.Register<NextImageMessage, string>(this, Token, async (obj, message) => await LoadNextImage());
		}

		public string Token { get; set; }
	}
}
