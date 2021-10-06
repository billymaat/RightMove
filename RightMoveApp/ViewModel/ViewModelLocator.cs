using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace RightMoveApp.ViewModel
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

		/// <summary>
		/// Gets the <see cref="ImageViewModel"/>
		/// </summary>
		public ImageViewModel ImageViewModel
			=> App.ServiceProvider.GetRequiredService<ImageViewModel>();
	}
}
