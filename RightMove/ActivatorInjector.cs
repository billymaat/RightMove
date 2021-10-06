using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove
{
	public interface IActivator
	{
		object CreateInstance(IServiceProvider serviceProvider, Type t);
		object CreateInstance(IServiceProvider serviceProvider, Type t, object[] constructorParams);
	}

	public class ActivatorInjector : IActivator
	{
		public object CreateInstance(IServiceProvider serviceProvider, Type t)
		{
			return ActivatorUtilities.CreateInstance(serviceProvider, t);
		}

		public object CreateInstance(IServiceProvider serviceProvider, Type t, object[] constructorParams)
		{
			return ActivatorUtilities.CreateInstance(serviceProvider, t, constructorParams);
		}
	}
}
