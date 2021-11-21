using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RightMove.DataTypes;

namespace RightMove.Db.Models
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
			Link = rightMoveProperty.Link;
			Price = rightMoveProperty.Price.ToString(CultureInfo.CurrentCulture);

			// initialise the dates
			DateAdded = rightMoveProperty.DateAdded.ToString("dd/MM/yyyy");
			DateReduced = rightMoveProperty.DateReduced.ToString("dd/MM/yyyy");
			Date = DateTime.Now.ToString("dd/MM/yyyy");
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

		public string DateAdded
		{
			get;
			set;
		}
		public string DateReduced { get; }

		/// <summary>
		/// Gets the Date the model was created
		/// </summary>
		public string Date 
		{ 
			get; 
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

		public List<int> Prices => SplitIntegers(Price);

		/// <summary>
		/// Gets the latest price - the last price added
		/// </summary>
		public int LatestPrice => Prices.Last(); 

		public List<string> Dates => SplitString(Date);

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
