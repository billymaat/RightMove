using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RightMove.DataTypes;
using RightMove.Desktop.Model;
using RightMove.Factory;
using RightMove.Services;

namespace RightMove.Desktop.Services
{
    public class RightMoveService
    {
        private readonly RightMoveParserFactory _parserFactory;
        private readonly Func<IPropertyPageParser> _propertyParserFactory;

        public RightMoveService(RightMoveParserFactory parserFactory,
            Func<IPropertyPageParser> propertyParserFactory)
        {
            _parserFactory = parserFactory ?? throw new ArgumentNullException(nameof(parserFactory));
            _propertyParserFactory = propertyParserFactory;
        }

        public async Task<RightMoveSearchItemCollection> GetRightMoveItems(SearchParams searchParams)
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
    }
}
