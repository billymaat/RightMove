using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace RightMove.Desktop.View
{
	/// <summary>
	/// Interaction logic for RightMoveImageView.xaml
	/// </summary>
	public partial class RightMoveImageView : UserControl
	{
		public RightMoveImageView()
		{
			InitializeComponent();
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
	}
}
