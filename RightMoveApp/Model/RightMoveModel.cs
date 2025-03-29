using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Messaging;
using RightMove.DataTypes;
using RightMove.Desktop.Helpers;
using RightMove.Desktop.Mappers;
using RightMove.Desktop.Messages;
using RightMove.Desktop.Services;
using RightMove.Services;

namespace RightMove.Desktop.Model
{
	public class RightMoveModel
	{
        private readonly RightMoveService _rightMoveService;
        private readonly RightMoveSearchHistoryWriter _searchHistoryWriter;
        private readonly RightMoveSearchHistoryReader _searchHistoryReader;
        private readonly IMessenger _messenger;

        public RightMoveModel(RightMoveService rightMoveService,
            RightMoveSearchHistoryWriter searchHistoryWriter,
			RightMoveSearchHistoryReader searchHistoryReader,
			IMessenger messenger)
		{
            _rightMoveService = rightMoveService;
            _searchHistoryWriter = searchHistoryWriter;
            _searchHistoryReader = searchHistoryReader;
            _messenger = messenger;
        }

        public void SetToken(string token)
        {
	        Token = token;
        }

        public string Token { get; set; }

        private List<RightMoveProperty> _rightMovePropertyItems;

        public List<RightMoveProperty> RightMovePropertyItems
		{
			get => _rightMovePropertyItems;
			set
			{
				_rightMovePropertyItems = value;
                _messenger.Send(new RightMovePropertyItemsUpdatedMessage()
	                {
	                    NewValue = value
	                }, 
		            Token);
            }
		}

        public List<SearchHistoryItem> SearchHistoryItems
        {
	        get => _searchHistoryItems;
	        set
	        {
		        _searchHistoryItems = value;
		        _messenger.Send(new SearchHistoryItemsUpdatedMessage()
			        {
				        NewValue = value
			        },
			        Token);
	        } 
        }

        private RightMoveProperty _rightMovePropertyFullSelectedItem;
		private List<SearchHistoryItem> _searchHistoryItems;

		public RightMoveProperty RightMovePropertyFullSelectedItem
		{
			get => _rightMovePropertyFullSelectedItem;
			set
			{
				_rightMovePropertyFullSelectedItem = value;
	                _messenger.Send(new RightMoveFullSelectedItemUpdatedMessage()
		                {
		                    NewValue = value
		                }, 
		                Token);
            }
		}

		public async Task Search(SearchParams searchParams, string text)
        {
            var historySearchItem = new SearchHistoryItem(DateTime.UtcNow, text, searchParams);
            var dto = historySearchItem.ToDto();
            _searchHistoryWriter.WriteSearchHistory(dto);

            var rightMoveItems = await _rightMoveService.GetRightMoveItems(searchParams);
            var items = rightMoveItems.ToList();
            RightMovePropertyItems = items;

            var searchHistory = _searchHistoryReader.ReadExistingHistory()
	            .Select(o => o.ToDomain());
            SearchHistoryItems = searchHistory.ToList();
		}

        public async Task UpdateSelectedRightMoveItem(int rightMoveId, CancellationToken cancellationToken)
        {
            var fullProperty = await _rightMoveService.GetFullRightMoveItem(rightMoveId, cancellationToken);
            RightMovePropertyFullSelectedItem = fullProperty;
		}
	}
}