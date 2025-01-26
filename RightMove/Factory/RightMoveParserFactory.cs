using System;
using Microsoft.Extensions.DependencyInjection;
using RightMove.DataTypes;
using RightMove.Services;

namespace RightMove.Factory
{
	public interface IRightMoveParserFactory
	{
		RightMoveParser CreateInstance(SearchParams searchParams);
	}

	public class RightMoveParserFactory : IRightMoveParserFactory
	{
		private readonly IServiceProvider _services;
		private readonly IActivator _activator;

		public RightMoveParserFactory(IActivator activator, IServiceProvider services)
		{
			_services = services;
			_activator = activator;
		}

		public RightMoveParser CreateInstance(SearchParams searchParams)
        {
			return (RightMoveParser)_activator.CreateInstance(_services,
				typeof(RightMoveParser),
				new object[] { searchParams });
		}
	}
}
