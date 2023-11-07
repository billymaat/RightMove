using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightMove.DataTypes;
using RightMove.Db.Entities;
using RightMove.Db.Services;
using RightMove.Factory;

namespace RightMoveConsole.Services
{
	public class SearchService : ISearchService
	{
		private readonly IRightMoveParserServiceFactory _rightMoveParserServiceFactory;
		private readonly IDatabaseService<RightMovePropertyEntity> _db;

		public SearchService(IRightMoveParserServiceFactory rightMoveParserServiceFactory,
			IDatabaseService<RightMovePropertyEntity> db)
		{
			_rightMoveParserServiceFactory = rightMoveParserServiceFactory;
			_db = db;
		}


		/// <summary>
		/// Do the search and update the database
		/// </summary>
		/// <param name="searchParams">the search params</param>
		/// <returns></returns>
		public async Task<RightMoveSearchItemCollection> Search(SearchParams searchParams)
		{
			var rightMoveService = _rightMoveParserServiceFactory.CreateInstance(searchParams);
			bool res = await rightMoveService.SearchAsync();

			if (!res)
			{
				// failed
				return null;
			}

			return rightMoveService.Results;
		}


		/// <summary>
		/// Get search params
		/// </summary>
		/// <returns>the search params</returns>
		private SearchParams GetSearchParams(string regionLocation)
		{
			SearchParams searchParams = new SearchParams()
			{
				RegionLocation = regionLocation,
				Sort = SortType.HighestPrice,
				MinBedrooms = 1,
				MaxBedrooms = 10,
				MinPrice = 0,
				MaxPrice = 20000000,
				PropertyType = PropertyTypeEnum.None
			};

			return searchParams;
		}
	}
}
