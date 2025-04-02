using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RightMove.DataTypes;
using RightMove.Desktop.Messages;
using RightMove.Desktop.Model;
using RightMove.Desktop.ViewModel.Commands;
using ServiceCollectionUtilities;

namespace RightMove.Desktop.ViewModel
{
    public class PropertyInfoViewModel : ObservableRecipient
    {
        private RightMoveModel _rightMoveModel;
        private readonly IFactory<RightMoveImageViewModel> _rightMoveImageViewModelFactory;
        private readonly IMessenger _messenger;

        private int _selectedImageIndex;
        // cancellation token
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public PropertyInfoViewModel(IFactory<RightMoveImageViewModel> rightMoveImageViewModelFactory,
	        IMessenger messenger)
        {
	        _rightMoveImageViewModelFactory = rightMoveImageViewModelFactory;
	        _messenger = messenger;
	        RightMoveImageVm = _rightMoveImageViewModelFactory.Create();
        }

		public void SetRightMoveModel(RightMoveModel rightMoveModel)
        {
	        _rightMoveModel = rightMoveModel;
        }

        public void SetRightMoveProperty(RightMoveProperty rightMoveProperty)
        {
	        RightMovePropertyFullSelectedItem = rightMoveProperty;
        }

        private void PrevImage()
        {
	        throw new NotImplementedException();
        }

        public void NextImage()
		{

		}

		private BitmapImage _displayedImage;

        /// <summary>
        /// Gets or sets the displayed image
        /// </summary>
        public BitmapImage DisplayedImage
        {
            get => _displayedImage;
            set => SetProperty(ref _displayedImage, value);
        }

        private bool _loadingImage;

        public bool LoadingImage
        {
            get => _loadingImage;
            set => SetProperty(ref _loadingImage, value);
        }

        private RightMoveProperty _rightMoveSelectedItem;
        /// <summary>
        /// Gets or sets the selected <see cref="RightMoveViewItem"/>
        /// </summary>
        public RightMoveProperty RightMoveSelectedItem
        {
            get => _rightMoveSelectedItem;
            set => SetProperty(ref _rightMoveSelectedItem, value);
        }

        /// <summary>
        /// Gets or sets the NextImageCommand
        /// </summary>
        public IAsyncCommand NextImageCommand
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the PrevImageCommand
        /// </summary>
        public IAsyncCommand PrevImageCommand
        {
            get;
            set;
        }

        // Tried to get this AsyncCommand to work but it wouldn't
        public ICommand UpdateImages
        {
            get;
            set;
        }


        /// <summary>
        /// Initialize a bunch of <see cref="ICommand"/>
        /// </summary>
        private void InitializeCommands()
        {
            PrevImageCommand = AsyncCommand.Create(() => ExecuteUpdatePrevImageAsync(null), () => CanExecuteUpdatePrevImage(null));
            NextImageCommand = AsyncCommand.Create(() => ExecuteUpdateNextImageAsync(null), () => CanExecuteUpdateNextImage(null));
        }

        /// <summary>
        /// Update the displayed image
        /// </summary>
        /// <param name="cancellationToken">the cancellation token</param>
        /// <returns></returns>
        private async Task<BitmapImage> UpdateImage(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            LoadingImage = true;

            try
            {
                //DisplayedImage = await _rightMoveModel.GetImage(_selectedImageIndex);
                // update the image view
                //UpdateImageIndexView();

                PrevImageCommand.RaiseCanExecuteChanged();
                NextImageCommand.RaiseCanExecuteChanged();
                return DisplayedImage;
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
                return null;
            }
            finally
            {
                LoadingImage = false;
            }
        }


        //private async Task UpdateFullSelectedItemAndImage(CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    await UpdateRightMovePropertyFullSelectedItem(cancellationToken);
        //    await UpdateImage(cancellationToken);
        //    LoadingImage = false;
        //}

        //private async Task UpdateRightMovePropertyFullSelectedItem(CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    _selectedImageIndex = 0;

        //    // update the right move full selected item
        //    await _rightMoveModel.UpdateSelectedRightMoveItem(RightMoveSelectedItem.RightMoveId, cancellationToken);
        //}

        private async Task<BitmapImage> ExecuteUpdateNextImageAsync(object arg1)
        {
            _selectedImageIndex++;
            _tokenSource.Cancel();

            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            var bitmap = await UpdateImage(token);
            return bitmap;
        }


        /// <summary>
        /// Can execute update next image
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanExecuteUpdateNextImage(object obj)
        {
            return RightMovePropertyFullSelectedItem != null && _selectedImageIndex != RightMovePropertyFullSelectedItem.ImageUrl.Length - 1;
        }

        private bool CanExecuteUpdatePrevImage(object obj)
        {
            return _selectedImageIndex > 0;
        }

        private async Task<BitmapImage> ExecuteUpdatePrevImageAsync(object arg1)
        {
            _selectedImageIndex--;
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();

            var token = _tokenSource.Token;
            var bitmap = await UpdateImage(token);
            return bitmap;
        }

        private void UpdateImageIndexView()
        {
            ImageIndexView = _selectedImageIndex < 0 || !HasImages
                ? null
                : $"Image {_selectedImageIndex + 1} / {RightMovePropertyFullSelectedItem.ImageUrl.Length}";
        }

        /// <summary>
        /// Gets a value indicating whether selected item has images available
        /// </summary>
        public bool HasImages => RightMovePropertyFullSelectedItem != null && RightMovePropertyFullSelectedItem.ImageUrl.Length > 0;

        private string _imageIndexView;

        public string ImageIndexView
        {
            get => _imageIndexView;
            set => SetProperty(ref _imageIndexView, value);
        }

        public string Description
        {
	        get => _description;
	        set => SetProperty(ref _description, value);
        }

        public RightMoveImageViewModel RightMoveImageVm
        {
	        get => _rightMoveImageVm;
	        set => SetProperty(ref _rightMoveImageVm, value);
        }

        private RightMoveProperty _rightMovePropertyFullSelectedItem;
        private string _description;
        private RightMoveImageViewModel _rightMoveImageVm;

        public RightMoveProperty RightMovePropertyFullSelectedItem
        {
            get => _rightMovePropertyFullSelectedItem;
            set
            {
	            if (SetProperty(ref _rightMovePropertyFullSelectedItem, value))
	            {
		            RightMoveImageVm.RightMoveProperty = value;
	            }
            }
        }

        //private void ExecuteUpdateImages(object arg)
        //{
        //    if (_selectedItemChangedTimer.IsEnabled)
        //    {
        //        _selectedItemChangedTimer.Stop();
        //    }

        //    _selectedItemChangedTimer.Start();
        //    LoadingImage = true;
        //}

        ///// <summary>
        ///// Can execute update images
        ///// </summary>
        ///// <param name="arg">the argument</param>
        ///// <returns>true if can execute, false otherwise</returns>
        //private bool CanExecuteUpdateImages(object arg)
        //{
        //    return true;
        //    //return RightMoveSelectedItem != null;
        //}
        public void SetToken(string token)
        {
	        Token = token;
	        RightMoveImageVm.SetToken(Token);
        }

		public string Token { get; set; }
    }
}
