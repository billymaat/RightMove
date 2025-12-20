using System;
using System.Threading;
using System.Threading.Tasks;
using RightMove.DataTypes;
using RightMove.Services;

namespace RightMove.Desktop.Services
{
    public class RightMoveService
    {
        private readonly RightMoveParser _rightMoveParser;

        public RightMoveService(RightMoveParser rightMoveParser)
        {
            _rightMoveParser = rightMoveParser ?? throw new ArgumentNullException(nameof(rightMoveParser));
        }

        public async Task<RightMoveSearchItemCollection> GetRightMoveItems(SearchParams searchParams)
        {
            var results = await _rightMoveParser.SearchAsync(searchParams);
            return results;
        }
    }
}
