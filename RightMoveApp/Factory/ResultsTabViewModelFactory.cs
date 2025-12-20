using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RightMove.DataTypes;
using RightMove.Desktop.Model;
using RightMove.Desktop.Services;
using RightMove.Desktop.View;
using RightMove.Desktop.ViewModel;
using RightMove.Services;

namespace RightMove.Desktop.Factory
{
	public class ResultsTabViewModelFactory
	{
		private readonly IServiceProvider _provider;
		public ResultsTabViewModelFactory(IServiceProvider provider)
		{
			_provider = provider;
		}

		public ResultsTabViewModel Create(List<RightMoveProperty> items, string title)
		{
			var rightMoveModel = new RightMoveModel()
			{
				RightMovePropertyItems = items
			};
			var rightMoveImageService = _provider.GetRequiredService<RightMoveImageService>();
			var rightMoveImageViewModel = new RightMoveImageViewModel(rightMoveImageService, rightMoveModel);
			var propertyPageParserService = _provider.GetRequiredService<PropertyPageParserService>();
			var nearbySoldPricesService = _provider.GetRequiredService<NearbySoldPricesService>();

			var propertyInfoViewModel =
				new PropertyInfoViewModel(rightMoveImageViewModel, propertyPageParserService, nearbySoldPricesService, rightMoveModel);
			var rightMoveModelService = new RightMoveModelService(rightMoveModel);

			var vm = new ResultsTabViewModel(propertyInfoViewModel, rightMoveModel, rightMoveModelService)
			{
				Title = title
			};

			return vm;
		}
	}
}
