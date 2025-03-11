using System;

namespace RightMove.DataTypes
{
    public class SearchHistoryItem
    {
        public DateTime CreatedAt { get; set; }
        public string DisplayText { get; set; }
        public SearchParams SearchParams { get; set; }
        
        public SearchHistoryItem(DateTime createdAt, string displayText, SearchParams searchParams)
        {
            CreatedAt = createdAt;
            DisplayText = displayText;
            SearchParams = searchParams;
        }
    }
}
