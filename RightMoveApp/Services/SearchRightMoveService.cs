using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightMove.DataTypes;
using RightMove.Desktop.Mappers;
using RightMove.Desktop.Model;

namespace RightMove.Desktop.Services
{
	public class SearchRightMoveService
	{
		private readonly SearchHistoryModel _searchHistoryModel;
		private readonly RightMoveSearchHistoryWriter _searchHistoryWriter;
		private readonly RightMoveSearchHistoryReader _searchHistoryReader;
		private readonly RightMoveService _rightMoveService;

		public SearchRightMoveService(SearchHistoryModel searchHistoryModel,
			RightMoveSearchHistoryWriter searchHistoryWriter,
			RightMoveSearchHistoryReader searchHistoryReader,
			RightMoveService rightMoveService)
		{
			_searchHistoryModel = searchHistoryModel;
			_searchHistoryWriter = searchHistoryWriter;
			_searchHistoryReader = searchHistoryReader;
			_rightMoveService = rightMoveService;
		}

		public async Task<List<RightMoveProperty>> Search(SearchParams searchParams, string text)
		{
			var historySearchItem = new SearchHistoryItem(DateTime.UtcNow, text, searchParams);
			var dto = historySearchItem.ToDto();
			_searchHistoryWriter.WriteSearchHistory(dto);

			var rightMoveItems = await _rightMoveService.GetRightMoveItems(searchParams);
			var items = rightMoveItems.ToList();
			var ret = items;

			var searchHistory = _searchHistoryReader.ReadExistingHistory()
				.Select(o => o.ToDomain());
			_searchHistoryModel.SearchHistoryItems = searchHistory.ToList();
			return ret;
		}
	}
}
