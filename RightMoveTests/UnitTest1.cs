using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using RightMove;
using RightMove.DataTypes;
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
		}

		[Test]
		public async Task ParseSearchPage()
		{
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
	}
}