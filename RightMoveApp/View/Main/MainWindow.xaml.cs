using MahApps.Metro.Controls;

namespace RightMove.Desktop.View.Main
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public MainWindow(MainViewModel mainViewModel)
		{
			InitializeComponent();
			DataContext = mainViewModel;
        }

		private void ResultsDataGrid_OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{

		}
	}
}
