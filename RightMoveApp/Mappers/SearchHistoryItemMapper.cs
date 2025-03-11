using RightMove.DataTypes;

namespace RightMove.Desktop.Mappers
{
    public static class SearchHistoryItemMapper
    {
        public static SearchHistoryItemDto ToDto(this SearchHistoryItem item)
        {
            return new SearchHistoryItemDto
            {
                CreatedAt = item.CreatedAt,
                DisplayText = item.DisplayText,
                SearchParams = item.SearchParams.ToDto()
            };
        }

        public static SearchHistoryItem ToDomain(this SearchHistoryItemDto dto)
        {
            return new SearchHistoryItem(
                dto.CreatedAt,
                dto.DisplayText,
                dto.SearchParams.ToDomain()
            );
        }
    }
}
