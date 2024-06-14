using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMove.Db.Entities
{
	public class DatePrice
	{
		public int Id
		{
			get;
			set;
		}

		public DateTime Date
		{
			get;
			set;
		}

		public int Price
		{
			get;
			set;
		}
	}
}
