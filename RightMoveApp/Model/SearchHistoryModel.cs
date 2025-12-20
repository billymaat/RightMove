using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightMove.DataTypes;

namespace RightMove.Desktop.Model
{
	public class SearchHistoryItemsUpdatedEventArgs : EventArgs
	{
		public List<SearchHistoryItem> SearchHistoryItems { get; set; }
	}

	public class SearchHistoryModel
	{
		private List<SearchHistoryItem> _searchHistoryItems;
		public event EventHandler<SearchHistoryItemsUpdatedEventArgs> SearchHistoryItemsUpdated;

		public List<SearchHistoryItem> SearchHistoryItems
		{
			get => _searchHistoryItems;
			set
			{
				_searchHistoryItems = value;
				SearchHistoryItemsUpdated?.Invoke(this, new SearchHistoryItemsUpdatedEventArgs()
				{
					SearchHistoryItems = value
				});
			}
		}
	}
}
