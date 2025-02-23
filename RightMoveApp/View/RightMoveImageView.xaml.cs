﻿using System;
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
        public event EventHandler NextImageClicked;
        public event EventHandler PrevImageClicked;

        private RightMoveImageService _rightMoveImageService;

        public RightMoveImageView()
        {
			InitializeComponent();
            _rightMoveImageService = new RightMoveImageService();

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
            nameof(LoadingImage), typeof(bool), typeof(RightMoveImageView), new PropertyMetadata(default(bool)));

        public bool LoadingImage
        {
            get { return (bool)GetValue(LoadingImageProperty); }
            set { SetValue(LoadingImageProperty, value); }
        }

        private async void OnPrevButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_imgIndex > 0)
            {
                _imgIndex--;
                await LoadImage(RightMoveProperty, _imgIndex);
            }

            PrevImageClicked?.Invoke(this, EventArgs.Empty);
        }

        private async void OnNextButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_imgIndex < RightMoveProperty.ImageUrl.Length - 2)
            {
                _imgIndex++;
                await LoadImage(RightMoveProperty, _imgIndex);
            }

            NextImageClicked?.Invoke(this, EventArgs.Empty);
        }

        private int _imgIndex = 0;

        private async Task ResetImage(RightMoveProperty rightMoveProperty)
        {
            _imgIndex = 0;
            await LoadImage(rightMoveProperty, _imgIndex);
        }

        private async Task LoadImage(RightMoveProperty rightMoveProperty, int imgIndex)
        {
            LoadingImage = true;
            var img = await _rightMoveImageService.GetImage(rightMoveProperty, imgIndex);
            Image = img;
            LoadingImage = false;
        }
    }
}
