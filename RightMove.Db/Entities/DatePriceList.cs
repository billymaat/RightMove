using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMove.Db.Entities
{
	public class DatePriceList
	{
		public List<DatePrice> DatePrices
		{
			get;
			set;
		} = new List<DatePrice>();
	}
}
