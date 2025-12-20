using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMove.DataTypes
{
	public class NearbySoldProperty
	{
		public string Address { get; set; }
		public int Bedrooms { get; set; }
		public int Bathrooms { get; set; }
		public string PropertyType { get; set; }
		public string Price { get; set; }
		public DateTime? DateSold { get; set; }
		public string Url { get; set; }
	}
}
