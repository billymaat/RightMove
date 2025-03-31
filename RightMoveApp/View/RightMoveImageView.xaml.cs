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
        public event EventHandler NextImageClicked;
        public event EventHandler PrevImageClicked;


        public RightMoveImageView()
        {
			InitializeComponent();
        }

        //     protected override void OnKeyDown(KeyEventArgs e)
   //     {
	  //      base.OnKeyDown(e);

			//switch (e.Key)
	  //      {
		 //       case Key.A:
			//        OnPrevButtonClicked(this, null);
			//        break;
		 //       case Key.S:
			//        OnNextButtonClicked(this, null);
			//        break;
	  //      }
   //     }
	}
}
