using Microsoft.Extensions.DependencyInjection;
using RightMove.Desktop.View.Main;

namespace RightMove.Desktop.ViewModel
{
	public class ViewModelLocator
	{
		public ViewModelLocator()
		{
		}

		/// <summary>
		/// Gets the <see cref="MainViewModel"/>
		/// </summary>
		public MainViewModel MainViewModel
			=> App.ServiceProvider.GetRequiredService<MainViewModel>();
	}
}
