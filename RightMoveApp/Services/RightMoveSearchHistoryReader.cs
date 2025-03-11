using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using RightMove.DataTypes;

namespace RightMove.Desktop.Services
{
    public class RightMoveSearchHistoryReader
    {
        public List<SearchHistoryItemDto> ReadExistingHistory()
        {
            if (!File.Exists(RightMoveSearchHistoryConfig.HistoryFilePath))
            {
                return new List<SearchHistoryItemDto>();
            }

            var json = File.ReadAllText(RightMoveSearchHistoryConfig.HistoryFilePath);
            return JsonSerializer.Deserialize<List<SearchHistoryItemDto>>(json) ?? new List<SearchHistoryItemDto>();
        }
    }
}
