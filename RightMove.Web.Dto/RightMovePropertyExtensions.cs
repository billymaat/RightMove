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
				Prices = new List<int>(entity.Prices),
				Dates = new List<DateTime>(entity.Dates),
				ResultsTableId = entity.ResultsTableId
			};
		}

		public static RightMovePropertyEntity ToEntity(this RightMovePropertyDto dto)
		{
			return new RightMovePropertyEntity
			{
				RightMovePropertyId = dto.RightMovePropertyId,
				RightMoveId = dto.RightMoveId,
				HouseInfo = dto.HouseInfo,
				Address = dto.Address,
				DateAdded = dto.DateAdded,
				DateReduced = dto.DateReduced,
				Date = dto.Date,
				Prices = new List<int>(dto.Prices),
				Dates = new List<DateTime>(dto.Dates),
				ResultsTableId = dto.ResultsTableId
				// ResultsTable is not included in DTO, so it's not converted
			};
		}
	}
}
