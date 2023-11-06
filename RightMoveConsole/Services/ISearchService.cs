using System.Threading.Tasks;
using RightMove.DataTypes;

namespace RightMoveConsole.Services;

public interface ISearchService
{
	/// <summary>
	/// Do the search and update the database
	/// </summary>
	/// <param name="searchParams">the search params</param>
	/// <returns></returns>
	Task<RightMoveSearchItemCollection> Search(SearchParams searchParams);
}