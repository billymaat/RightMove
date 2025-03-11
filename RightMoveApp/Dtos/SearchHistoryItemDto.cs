using System;

namespace RightMove.DataTypes
{
    public class SearchHistoryItemDto
    {
        public DateTime CreatedAt { get; set; }
        public string DisplayText { get; set; }
        public SearchParamsDto SearchParams { get; set; }
    }
}
