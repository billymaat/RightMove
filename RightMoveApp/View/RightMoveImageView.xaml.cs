using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RightMove.DataTypes;
using RightMove.Desktop.Services;

namespace RightMove.Desktop.View
{
    /// <summary>
    /// Interaction logic for RightMoveImageView.xaml
    /// </summary>
    public partial class RightMoveImageView : UserControl
	{
        private int _imgIndex;

        public event EventHandler NextImageClicked;
        public event EventHandler PrevImageClicked;

        private readonly RightMoveImageService _rightMoveImageService;

        public RightMoveImageView()
        {
			InitializeComponent();
            _rightMoveImageService = new RightMoveImageService();

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

        public static readonly DependencyProperty RightMovePropertyProperty = DependencyProperty.Register(
            nameof(RightMoveProperty), typeof(RightMoveProperty), typeof(RightMoveImageView), new PropertyMetadata(default(RightMoveProperty), OnRightMovePropertyChanged));

        private static void OnRightMovePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is RightMoveProperty rightMoveProperty)
            {
                var view = d as RightMoveImageView;

                view?.ResetImage(rightMoveProperty);
            }
        }

        public RightMoveProperty RightMoveProperty
        {
            get { return (RightMoveProperty)GetValue(RightMovePropertyProperty); }
            set { SetValue(RightMovePropertyProperty, value); }
        }

        public static readonly DependencyProperty ImageIndexViewProperty = DependencyProperty.Register(
            nameof(ImageIndexView), typeof(string), typeof(RightMoveImageView), new PropertyMetadata(default(string)));

        public string ImageIndexView
        {
            get { return (string)GetValue(ImageIndexViewProperty); }
            set { SetValue(ImageIndexViewProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image), typeof(BitmapImage), typeof(RightMoveImageView), new PropertyMetadata(default(BitmapImage)));

        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty PrevImageProperty = DependencyProperty.Register(
            nameof(PrevImage), typeof(ICommand), typeof(RightMoveImageView), new PropertyMetadata(default(ICommand)));

        public ICommand PrevImage
        {
            get { return (ICommand)GetValue(PrevImageProperty); }
            set { SetValue(PrevImageProperty, value); }
        }

        public static readonly DependencyProperty NextImageProperty = DependencyProperty.Register(
            nameof(NextImage), typeof(ICommand), typeof(RightMoveImageView), new PropertyMetadata(default(ICommand)));

        public ICommand NextImage
        {
            get { return (ICommand)GetValue(NextImageProperty); }
            set { SetValue(NextImageProperty, value); }
        }

        public static readonly DependencyProperty LoadingImageProperty = DependencyProperty.Register(
            nameof(LoadingImage), typeof(bool), typeof(RightMoveImageView), new PropertyMetadata(default(bool), OnLoadingImageUpdated));

        private static void OnLoadingImageUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (RightMoveImageView)d;
            ctrl.UpdateButtonsEnabled();
        }

        public bool LoadingImage
        {
            get { return (bool)GetValue(LoadingImageProperty); }
            set { SetValue(LoadingImageProperty, value); }
        }

        public static readonly DependencyProperty NextButtonEnabledProperty = DependencyProperty.Register(
            nameof(NextButtonEnabled), typeof(bool), typeof(RightMoveImageView), new PropertyMetadata(true));

        public bool NextButtonEnabled
        {
            get { return (bool)GetValue(NextButtonEnabledProperty); }
            set { SetValue(NextButtonEnabledProperty, value); }
        }

        public static readonly DependencyProperty PrevButtonEnabledProperty = DependencyProperty.Register(
            nameof(PrevButtonEnabled), typeof(bool), typeof(RightMoveImageView), new PropertyMetadata(default(bool)));

        public bool PrevButtonEnabled
        {
            get { return (bool)GetValue(PrevButtonEnabledProperty); }
            set { SetValue(PrevButtonEnabledProperty, value); }
        }

        private async void OnPrevButtonClicked(object sender, RoutedEventArgs e)
        {
			await LoadPrevImage();

			PrevImageClicked?.Invoke(this, EventArgs.Empty);
        }

        private async void OnNextButtonClicked(object sender, RoutedEventArgs e)
        {
	        await LoadNextImage();

            NextImageClicked?.Invoke(this, EventArgs.Empty);
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
            SetValue(PrevButtonEnabledProperty, !LoadingImage && ImgIndex > 0);
        }

        private void UpdateNextEnabled()
        {
            // Automatically updates IsNextDisabled
            SetValue(NextButtonEnabledProperty, !LoadingImage && ImgIndex < RightMoveProperty.ImageUrl.Length - 1);
        }

        private void RightMoveImageView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += OnKeyDown;
		}

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
	        switch (e.Key)
	        {
				case Key.A:
					OnPrevButtonClicked(this, null);
					break;
				case Key.S:
					OnNextButtonClicked(this, null);
					break;
			}
		}
	}
}
