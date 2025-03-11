using System;

namespace RightMove.DataTypes
{
    public class SearchParamsDto
    {
        public string OutcodeLocation { get; set; }
        public string RegionLocation { get; set; }
        public int MinBedrooms { get; set; }
        public int MaxBedrooms { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public PropertyTypeEnum PropertyType { get; set; }
        public SortType Sort { get; set; }
        public double Radius { get; set; }
        public DateTime SearchDate { get; set; } = DateTime.Now;
    }
}
