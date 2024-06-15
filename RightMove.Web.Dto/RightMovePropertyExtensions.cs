using RightMove.Db.Entities;

namespace RightMove.Web.Dto
{
	public static class RightMovePropertyExtensions
	{
		public static RightMovePropertyDto ToDto(this RightMovePropertyEntity entity)
		{
			return new RightMovePropertyDto
			{
				RightMovePropertyId = entity.RightMovePropertyId,
				RightMoveId = entity.RightMoveId,
				HouseInfo = entity.HouseInfo,
				Address = entity.Address,
				DateAdded = entity.DateAdded,
				DateReduced = entity.DateReduced,
				Date = entity.Date,
				Prices = new List<DatePriceDto>(entity.Prices.Select(o => new DatePriceDto()
				{
					Date = o.Date,
					Price = o.Price
				})),
				ResultsTableId = entity.ResultsTableId
			};
		}
	}
}
