using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using RightMove.DataTypes;
using RightMove.Services;

namespace RightMove.Factory
{
	public interface IRightMoveParserServiceFactory
	{
		RightMoveParserService CreateInstance(SearchParams searchParams);
	}

	public class RightMoveParserServiceFactory : IRightMoveParserServiceFactory
	{
		private readonly IServiceProvider _services;
		private readonly IActivator _activator;

		public RightMoveParserServiceFactory(IActivator activator, IServiceProvider services)
		{
			_services = services;
			_activator = activator;
		}

		public RightMoveParserService CreateInstance(SearchParams searchParams)
		{
			return (RightMoveParserService)_activator.CreateInstance(_services,
				typeof(RightMoveParserService),
				new object[] { searchParams });
		}
	}
}
