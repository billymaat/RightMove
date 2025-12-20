using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RightMove.DataTypes;
using RightMove.Desktop.Model;
using RightMove.Desktop.Services;

namespace RightMove.Desktop.View
{
	public class RightMoveImageViewModel : ObservableRecipient
	{
		private readonly RightMoveImageService _rightMoveImageService;
		private readonly RightMoveModel _rightMoveModel;

		public RightMoveImageViewModel(RightMoveImageService rightMoveImageService, RightMoveModel rightMoveModel)
		{
			_rightMoveImageService = rightMoveImageService;
			_rightMoveModel = rightMoveModel;

			_rightMoveModel.RightMoveSelectedItemUpdated +=(r, m) =>
			{
				RightMoveSelectedItem = m.SelectedItem;
			};

			PrevImageCommand = new AsyncRelayCommand(LoadPrevImageAsync, CanExecutePrevImage);
			NextImageCommand = new AsyncRelayCommand(LoadNextImageAsync, CanExecuteNextImage);
		}

		private bool CanExecuteNextImage()
		{
			if (RightMoveSelectedItem == null)
			{
				return false;
			}

			return ImgIndex < RightMoveSelectedItem.ImageUrl.Length - 1;
		}

		private bool CanExecutePrevImage()
		{
			if (RightMoveSelectedItem == null)
			{
				return false;
			}

			return ImgIndex > 0;
		}

		private RightMoveProperty _rightMoveSelectedItem;
		private BitmapImage _image;
		private bool _loadingImage;
		private int _imgIndex;
		private string _imageIndexView;

		public RightMoveProperty RightMoveSelectedItem
		{
			get => _rightMoveSelectedItem;
			set
			{
				if (SetProperty(ref _rightMoveSelectedItem, value))
				{
					OnRightMoveSelectedItemUpdated();
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
			if (RightMoveSelectedItem == null)
			{
				ImageIndexView = "-";
				return;
			}

			ImageIndexView = $"{ImgIndex + 1} of {RightMoveSelectedItem.ImageUrl.Length}";
		}

		public string ImageIndexView
		{
			get => _imageIndexView;
			set => SetProperty(ref _imageIndexView, value);
		}

		private void OnRightMoveSelectedItemUpdated()
		{
			ResetImage();
			PrevImageCommand.NotifyCanExecuteChanged();
			NextImageCommand.NotifyCanExecuteChanged();
		}

		public BitmapImage Image
		{
			get => _image;
			set => SetProperty(ref _image, value);
		}

		public bool LoadingImage
		{
			get => _loadingImage;
			set => SetProperty(ref _loadingImage, value);
		}

		public AsyncRelayCommand PrevImageCommand { get; }
		public AsyncRelayCommand NextImageCommand { get; }

		private async Task LoadPrevImageAsync()
		{
			if (ImgIndex > 0)
			{
				ImgIndex--;
				await LoadImageAsync(ImgIndex);
			}

			NextImageCommand.NotifyCanExecuteChanged();
		}

		private async Task LoadNextImageAsync()
		{
			if (ImgIndex < RightMoveSelectedItem.ImageUrl.Length - 1)
			{
			  ImgIndex++;
			  await LoadImageAsync(ImgIndex);
			}
			PrevImageCommand.NotifyCanExecuteChanged();
		}

		private void ResetImage()
		{
			ImgIndex = 0;
			_ = LoadImageAsync(ImgIndex);
		}

		private async Task LoadImageAsync(int imgIndex)
		{
			if (RightMoveSelectedItem == null)
			{
				LoadingImage = false;
				Image = null;
				return;
			}

			if (RightMoveSelectedItem.ImageUrl == null || RightMoveSelectedItem.ImageUrl.Length == 0)
			{
				LoadingImage = false;
				Image = null;
				return;
			}

			LoadingImage = true;
			try
			{
				var img = await _rightMoveImageService.GetImage(RightMoveSelectedItem, imgIndex);
				Image = img;
			}
			catch (Exception ex)
			{
				// Log or handle the exception
				Image = null;
			}
			finally
			{
				LoadingImage = false;
			}
		}
	}
}
