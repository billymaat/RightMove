using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using RightMove;
using RightMove.DataTypes;
using RightMove.Factory;
using RightMove.Services;

namespace RightMoveTests
{
	public class Tests
	{
		private IHttpService CreateHttpService()
		{
			return new HttpService();
		}

		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void Test1()
		{
			Assert.Pass();
		}

		[Test]
		public void ParseSearchPage_Null()
		{
			RightMoveParserFactory factory = CreateRightMoveParserServiceFactory();
			Assert.That(() => factory.CreateInstance(null), Throws.ArgumentNullException);
		}

		[Test]
		public async Task ParseSearchPage()
		{
			SearchParams searchParams = CreateSearchParams();

			var rightMoveParserServiceFactory = CreateRightMoveParserServiceFactory();
			var rightMoveParserService = rightMoveParserServiceFactory.CreateInstance(searchParams);

			await rightMoveParserService.SearchAsync();
			Debug.Assert(rightMoveParserService.Results.Count > 0);
		}

		/// <summary>
		/// Create a <see cref="RightMoveParserFactory"/>
		/// </summary>
		/// <returns>A <see cref="RightMoveParserFactory"/></returns>
		private RightMoveParserFactory CreateRightMoveParserServiceFactory()
		{
			IHttpService httpService = CreateHttpService();

			// create a mock serviceProvider
			var serviceProvider = new Mock<IServiceProvider>();

			// mock activators for factories
			var activatorSearchPageParserServiceFactory = new Mock<IActivator>();
			var activatorRightMovePropertyFactory = new Mock<IActivator>();
			var activatorRightMoveParserServiceFactory = new Mock<IActivator>();

			RightMovePropertyFactory rightMovePropertyFactory = new RightMovePropertyFactory(httpService);

			activatorSearchPageParserServiceFactory.Setup(a => a.CreateInstance(serviceProvider.Object, It.IsAny<Type>()))
				.Returns<IServiceProvider, Type>((sp, t)
				=> new SearchPageParserService(httpService, null, rightMovePropertyFactory));

			activatorRightMovePropertyFactory.Setup(a => a.CreateInstance(serviceProvider.Object, It.IsAny<Type>()))
				.Returns<IServiceProvider, Type>((sp, t)
				=> new RightMoveProperty(httpService));

			SearchPageParserServiceFactory searchPageParserServiceFactory = new SearchPageParserServiceFactory(activatorSearchPageParserServiceFactory.Object, serviceProvider.Object);
			var instance = searchPageParserServiceFactory.CreateInstance();

			activatorRightMoveParserServiceFactory.Setup(a => a.CreateInstance(serviceProvider.Object, It.IsAny<Type>(), It.IsAny<object[]>()))
				.Returns<IServiceProvider, Type, object[]>((sp, t, o) => new RightMoveParser(searchPageParserServiceFactory,
				null,
				(SearchParams)o[0]));

			RightMoveParserFactory rightMoveParserServiceFactory = new RightMoveParserFactory(activatorRightMoveParserServiceFactory.Object, serviceProvider.Object);

			return rightMoveParserServiceFactory;
		}

		/// <summary>
		/// Create some search params
		/// </summary>
		/// <returns>Creates search params</returns>
		private SearchParams CreateSearchParams()
		{
			SearchParams searchParams = new SearchParams()
			{
				RegionLocation = "Ashton-Under-Lyne, Greater Manchester",
				MinBedrooms = 0,
				MaxBedrooms = 5,
				MinPrice = 100000,
				MaxPrice = 10000000,
				Sort = SortType.HighestPrice,
				Radius = 0
			};

			return searchParams;
		}

		[Test]
		public void ParsePage()
		{

		}
	}
}