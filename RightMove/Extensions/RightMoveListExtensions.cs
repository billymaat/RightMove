using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RightMove.DataTypes;
using RightMove.Services;

namespace RightMove.Extensions
{
    public static class RightMoveListExtensions
    {
        public static double AveragePrice(this IEnumerable<RightMoveProperty> properties)
        {
            var propertiesList = properties.ToList();
            var propertiesWithPrice = propertiesList.Where(o => o.Price != RightMoveParser.PriceNotSet);
            if (!propertiesWithPrice.Any())
            {
                return double.MinValue;
            }

            return propertiesWithPrice
                .Select(o => o.Price)
                .Average();
        }
    }
}
