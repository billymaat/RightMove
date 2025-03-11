using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightMove.DataTypes;

namespace RightMove.Desktop.Mappers
{
    public static class SearchParamsExtensions
    {
        public static SearchParamsDto ToDto(this SearchParams searchParams)
        {
            return new SearchParamsDto
            {
                OutcodeLocation = searchParams.OutcodeLocation,
                RegionLocation = searchParams.RegionLocation,
                MinBedrooms = searchParams.MinBedrooms,
                MaxBedrooms = searchParams.MaxBedrooms,
                MinPrice = searchParams.MinPrice,
                MaxPrice = searchParams.MaxPrice,
                PropertyType = searchParams.PropertyType,
                Sort = searchParams.Sort,
                Radius = searchParams.Radius
            };
        }
    }
}
