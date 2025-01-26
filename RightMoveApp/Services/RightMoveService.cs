using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightMove.DataTypes;
using RightMove.Factory;

namespace RightMove.Desktop.Services
{
    public class RightMoveService
    {
        private readonly RightMoveParserFactory _parserFactory;

        public RightMoveService(RightMoveParserFactory parserFactory)
        {
            _parserFactory = parserFactory ?? throw new ArgumentNullException(nameof(parserFactory));
        }

        public async Task<RightMoveSearchItemCollection> GetRightMoveItems(SearchParams searchParams)
        {
            var parser = _parserFactory.CreateInstance(searchParams);
            await parser.SearchAsync();
            return parser.Results;
        }
    }
}
