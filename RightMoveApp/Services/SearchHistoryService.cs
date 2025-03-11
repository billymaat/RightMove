using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightMove.DataTypes;
using RightMove.Desktop.Mappers;

namespace RightMove.Desktop.Services
{
	public class SearchHistoryService
	{
		private readonly RightMoveSearchHistoryReader _reader;

		public SearchHistoryService(RightMoveSearchHistoryReader reader)
		{
			_reader = reader;
		}

		public IEnumerable<SearchHistoryItem> GetItems()
		{
			var history = _reader.ReadExistingHistory()
				.Select(o => o.ToDomain());
			return history;
		}
	}
}
