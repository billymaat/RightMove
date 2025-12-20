using System.Threading.Tasks;
using RightMove.DataTypes;
using RightMove.Services;

namespace RightMoveConsole.Services
{
	public class SearchService : ISearchService
	{
		private readonly RightMoveParser _rightMoveParser;

		public SearchService(RightMoveParser rightMoveParser)
		{
			_rightMoveParser = rightMoveParser;
		}

		/// <summary>
		/// Do the search
		/// </summary>
		/// <param name="searchParams">the search params</param>
		/// <returns></returns>
		public async Task<RightMoveSearchItemCollection> Search(SearchParams searchParams)
		{
			var res = await _rightMoveParser.SearchAsync(searchParams);

			return res;
		}
	}
}
