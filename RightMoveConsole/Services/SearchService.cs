using System.Threading.Tasks;
using RightMove.DataTypes;
using RightMove.Factory;

namespace RightMoveConsole.Services
{
	public class SearchService : ISearchService
	{
		private readonly IRightMoveParserFactory _rightMoveParserFactory;

		public SearchService(IRightMoveParserFactory rightMoveParserFactory)
		{
			_rightMoveParserFactory = rightMoveParserFactory;
		}

		/// <summary>
		/// Do the search
		/// </summary>
		/// <param name="searchParams">the search params</param>
		/// <returns></returns>
		public async Task<RightMoveSearchItemCollection> Search(SearchParams searchParams)
		{
			var rightMoveService = _rightMoveParserFactory.CreateInstance(searchParams);
			bool res = await rightMoveService.SearchAsync();

			if (!res)
			{
				// failed
				return null;
			}

			return rightMoveService.Results;
		}
	}
}
