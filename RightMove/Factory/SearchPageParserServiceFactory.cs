using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RightMove.Services;

namespace RightMove.Factory
{
	public class SearchPageParserServiceFactory
	{
		public readonly IServiceProvider _services;
		public readonly IActivator _activator;

		public SearchPageParserServiceFactory(IActivator activator, IServiceProvider services)
		{
			_services = services;
			_activator = activator;
		}

		//public Task<SearchPageParserService> CreateInstance()
		//{
		//	return Task.FromResult<SearchPageParserService>(ActivatorUtilities.CreateInstance<SearchPageParserService>(_services));
		//}

		public SearchPageParserService CreateInstance()
		{
			return (SearchPageParserService)_activator.CreateInstance(_services, typeof(SearchPageParserService));
		}
	}
}
