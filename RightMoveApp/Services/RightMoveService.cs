using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using RightMove.DataTypes;
using RightMove.Desktop.Mappers;
using RightMove.Desktop.Model;
using RightMove.Factory;
using RightMove.Services;
using ServiceCollectionUtilities;

namespace RightMove.Desktop.Services
{
    public class RightMoveService
    {
        private readonly RightMoveParserFactory _parserFactory;
        private readonly Func<IPropertyPageParser> _propertyParserFactory;
        private readonly RightMoveSearchHistoryWriter _searchHistoryWriter;
        private readonly RightMoveSearchHistoryReader _searchHistoryReader;
        private readonly IFactory<RightMoveModel> _rightMoveModelFactory;

        private readonly IMessenger _messenger;
        private readonly RightMoveModel _rightMoveModel;
		public RightMoveService(RightMoveParserFactory parserFactory,
            Func<IPropertyPageParser> propertyParserFactory,
            RightMoveSearchHistoryWriter searchHistoryWriter,
            RightMoveSearchHistoryReader searchHistoryReader,
            IFactory<RightMoveModel> rightMoveModelFactory,
			IMessenger messenger)
        {
	        _searchHistoryWriter = searchHistoryWriter;
	        _searchHistoryReader = searchHistoryReader;
	        _rightMoveModelFactory = rightMoveModelFactory;
	        _messenger = messenger;
	        _parserFactory = parserFactory ?? throw new ArgumentNullException(nameof(parserFactory));
            _propertyParserFactory = propertyParserFactory;
            _searchHistoryWriter = searchHistoryWriter;
            _searchHistoryReader = searchHistoryReader;
            _rightMoveModel = _rightMoveModelFactory.Create();
        }

        public RightMoveModel GetRightMoveModel()
		{
			return _rightMoveModel;
		}

		public async Task Search(SearchParams searchParams, string text)
		{
			var historySearchItem = new SearchHistoryItem(DateTime.UtcNow, text, searchParams);
			var dto = historySearchItem.ToDto();
			_searchHistoryWriter.WriteSearchHistory(dto);

			var rightMoveItems = await GetRightMoveItems(searchParams);
			var items = rightMoveItems.ToList();
			_rightMoveModel.RightMovePropertyItems = items;

			var searchHistory = _searchHistoryReader.ReadExistingHistory()
				.Select(o => o.ToDomain());
			_rightMoveModel.SearchHistoryItems = searchHistory.ToList();
		}

		private async Task<RightMoveSearchItemCollection> GetRightMoveItems(SearchParams searchParams)
        {
            var parser = _parserFactory.CreateInstance(searchParams);
            await parser.SearchAsync();
            return parser.Results;
        }

        public async Task<RightMoveProperty> GetFullRightMoveItem(int rightMoveId, CancellationToken cancellationToken)
        {
            IPropertyPageParser parser = _propertyParserFactory();

            await parser.ParseRightMovePropertyPageAsync(rightMoveId, cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            return parser.RightMoveProperty;
        }

        public async Task UpdateSelectedRightMoveItem(int rightMoveId, CancellationToken cancellationToken)
        {
	        var fullProperty = await GetFullRightMoveItem(rightMoveId, cancellationToken);
	        _rightMoveModel.RightMovePropertyFullSelectedItem = fullProperty;
        }

        public void SetToken(string token)
        {
	        _rightMoveModel.SetToken(token);
		}
    }
}
