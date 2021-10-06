using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RightMove.DataTypes;

namespace RightMoveConsole.Models
{
	public class RightMovePropertyModel
	{
		public RightMovePropertyModel()
		{
		}

		public RightMovePropertyModel(RightMoveProperty rightMoveProperty)
		{
			RightMoveId = rightMoveProperty.RightMoveId;
			HouseInfo = rightMoveProperty.HouseInfo;
			Address = rightMoveProperty.Address;
			Date = rightMoveProperty.Date.Date.ToString(CultureInfo.CurrentCulture);
			Link = rightMoveProperty.Link;
			Price = rightMoveProperty.Price.ToString(CultureInfo.CurrentCulture);
		}

		public int Id
		{
			get;
			set;
		}
		
		public int RightMoveId
		{
			get;
			set;
		}
		
		public string HouseInfo
		{
			get;
			set;
		}
		
		public string Address
		{
			get;
			set;
		}

		public string Date
		{
			get;
			set;
		}

		public string Link
		{
			get;
			set;
		}

		public string Price
		{
			get;
			set;
		}
		
		public string DateUpdated
		{
			get;
			set;
		}
		
		public List<int> Prices => SplitIntegers(Price);

		public List<string> DatesUpdated => SplitString(DateUpdated);
		
		private static List<string> SplitString(string s)
		{
			if (s is null)
			{
				return new List<string>();
			}

			return s.Split("|").ToList();
		}

		private static List<int> SplitIntegers(string s)
		{
			var lst = SplitString(s);

			return lst.Select(p =>
				{
					int.TryParse(p, out int price);
					return price;
				}).ToList();
		}
	}
}
