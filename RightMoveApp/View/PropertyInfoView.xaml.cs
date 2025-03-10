using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using RightMove.DataTypes;

namespace RightMove.Desktop.View
{
    /// <summary>
    /// Interaction logic for PropertyInfoView.xaml
    /// </summary>
    public partial class PropertyInfoView : UserControl
    {
        public PropertyInfoView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty RightMovePropertyProperty = DependencyProperty.Register(
            nameof(RightMoveProperty), typeof(RightMoveProperty), typeof(PropertyInfoView), new PropertyMetadata(default(RightMoveProperty)));

        public RightMoveProperty RightMoveProperty
        {
            get { return (RightMoveProperty)GetValue(RightMovePropertyProperty); }
            set { SetValue(RightMovePropertyProperty, value); }
        }


        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(PropertyInfoView), new PropertyMetadata(default(string)));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DisplayedImageProperty = DependencyProperty.Register(
            nameof(DisplayedImage), typeof(BitmapImage), typeof(PropertyInfoView), new PropertyMetadata(default(BitmapImage)));

        public BitmapImage DisplayedImage
        {
            get { return (BitmapImage)GetValue(DisplayedImageProperty); }
            set { SetValue(DisplayedImageProperty, value); }
        }
    }
}
