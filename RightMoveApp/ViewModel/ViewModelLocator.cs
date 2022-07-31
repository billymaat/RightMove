using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using RightMoveApp.View.Main;

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
	}
}
