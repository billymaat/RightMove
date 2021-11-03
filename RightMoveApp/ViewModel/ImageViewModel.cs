using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using RightMove;
using RightMove.DataTypes;
using RightMove.Services;
using RightMoveApp.Model;
using RightMoveApp.Services;
using RightMoveApp.ViewModel.Commands;

namespace RightMoveApp.ViewModel
{
	public class ImageViewModel : ViewModelBase, IActivable
	{
		private RightMoveProperty _rightMoveProperty;
		private int _selectedImageIndex = 0;

		public ImageViewModel()
		{
			// NextImageAsyncCommand = new AsyncCommand<object>(ExecuteNextImage, CanExecuteNextImage);
			// PrevImageAsyncCommand = new AsyncCommand<object>(ExecutePrevImage, CanExecutePrevImage);
		}

		private bool CanExecutePrevImage(object arg)
		{
			if (_rightMoveProperty is null)
			{
				return false;
			}

			return _selectedImageIndex > 0;
		}

		private async Task ExecutePrevImage(object arg)
		{
			_selectedImageIndex--;
			await UpdateImage();
		}

		private bool CanExecuteNextImage(object arg)
		{
			if (_rightMoveProperty is null)
			{
				return false;
			}

			return _selectedImageIndex != _rightMoveProperty.ImageUrl.Length - 1;
		}

		private async Task ExecuteNextImage(object arg)
		{
			_selectedImageIndex++;
			await UpdateImage();
		}

		private int RightMoveId
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the selected <see cref="RightMoveViewItem"/>
		/// </summary>
		public RightMoveProperty RightMoveSelectedItem
		{
			get => _rightMoveProperty;
			set => Set(ref _rightMoveProperty, value);
		}

		public ICommand NextImageAsyncCommand
		{
			get;
			set;
		}

		public ICommand PrevImageAsyncCommand
		{
			get;
			set;
		}

		public async Task ActivateAsync(object parameter)
		{
			if (parameter is int propertyId)
			{
				RightMoveId = propertyId;
			}
			/*
			PropertyPageParser parser = new PropertyPageParser(RightMoveId);
			await parser.ParseRightMovePropertyPageAsync();
			_rightMoveProperty = parser.RightMoveProperty;
			await UpdateImage();
			*/
		}

		private async Task UpdateImage()
		{
			byte[] imageArr = _rightMoveProperty.GetImage(_selectedImageIndex);
			if (imageArr is null)
			{
				return;
			}

			var bitmapImage = ToImage(imageArr);
			DisplayedImage = bitmapImage;
		}

		private BitmapImage ToImage(byte[] array)
		{
			using (var ms = new System.IO.MemoryStream(array))
			{
				var image = new BitmapImage();
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad; // here
				image.StreamSource = ms;
				image.EndInit();
				return image;
			}
		}

		private BitmapImage _displayedImage;

		public BitmapImage DisplayedImage
		{
			get
			{
				return _displayedImage;
			}
			set
			{
				Set(ref _displayedImage, value);
			}
		}
	}
}
