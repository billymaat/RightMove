using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightMove.DataTypes;
using System.Text.Json;
using System.IO;

namespace RightMove.Desktop.Services
{
    public class RightMoveSearchHistoryWriter
    {
        private readonly RightMoveSearchHistoryReader _reader;

        public RightMoveSearchHistoryWriter(RightMoveSearchHistoryReader reader)
        {
            _reader = reader;
            var directory = Path.GetDirectoryName(RightMoveSearchHistoryConfig.HistoryFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }
        }

        public void WriteSearchHistory(SearchHistoryItemDto historyItem)
        {
            var history = _reader.ReadExistingHistory();
            history.Insert(0, historyItem);
            
            if (history.Count > RightMoveSearchHistoryConfig.MaxHistoryItems)
                history = history.Take(RightMoveSearchHistoryConfig.MaxHistoryItems).ToList();

            var json = JsonSerializer.Serialize(history, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            
            File.WriteAllText(RightMoveSearchHistoryConfig.HistoryFilePath, json);
        }
    }
}
