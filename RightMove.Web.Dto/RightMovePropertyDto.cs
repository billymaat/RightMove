using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMove.Web.Dto
{
	public class RightMovePropertyDto
	{
		public int RightMovePropertyId { get; set; }
		public int RightMoveId { get; set; }
		public string HouseInfo { get; set; }
		public string Address { get; set; }
		public DateTime DateAdded { get; set; }
		public DateTime DateReduced { get; set; }
		public DateTime Date { get; set; }
		public List<int> Prices { get; set; }
		public List<DateTime> Dates { get; set; }
		public int ResultsTableId { get; set; }
		// Note: We're excluding the ResultsTable object to keep the DTO simple
	}
}
