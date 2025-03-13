using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightMove.DataTypes;
using System.Text.Json;
using System.IO;
using RightMove.Desktop.Mappers;

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
            if (history.Count > 0)
            {
	            var historySearch = historyItem.SearchParams.ToDomain();
	            var item = history[0].SearchParams.ToDomain();

	            if (AreSearchParamsEqual(historySearch, item))
	            {
		            return;
	            }
            }

            history.Insert(0, historyItem);
            
            if (history.Count > RightMoveSearchHistoryConfig.MaxHistoryItems)
                history = history.Take(RightMoveSearchHistoryConfig.MaxHistoryItems).ToList();

            var json = JsonSerializer.Serialize(history, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            
            File.WriteAllText(RightMoveSearchHistoryConfig.HistoryFilePath, json);
        }

		private bool AreSearchParamsEqual(SearchParams searchParams1, SearchParams searchParams2)
		{
			return searchParams1.OutcodeLocation == searchParams2.OutcodeLocation
			       && searchParams1.RegionLocation == searchParams2.RegionLocation
			       && searchParams1.MinBedrooms == searchParams2.MinBedrooms
			       && searchParams1.MaxBedrooms == searchParams2.MaxBedrooms
			       && searchParams1.MinPrice == searchParams2.MinPrice
			       && searchParams1.MaxPrice == searchParams2.MaxPrice
			       && searchParams1.PropertyType == searchParams2.PropertyType
			       && searchParams1.Sort == searchParams2.Sort
			       && searchParams1.Radius == searchParams2.Radius;
		}
    }
}
