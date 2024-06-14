using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RightMove.Db.Entities
{
	public class RightMovePropertyEntity
	{
		public RightMovePropertyEntity()
		{
		}
		
		[Key]
		public int RightMovePropertyId
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

		public DateTime DateAdded
		{
			get;
			set;
		}
		public DateTime DateReduced 
		{ 
			get;
			set;
		}

		/// <summary>
		/// Gets the Date the model was created
		/// </summary>
		public DateTime Date
		{
			get;
			set;
		}

		public List<DatePrice> Prices
		{
			get;
			set;
		} = new List<DatePrice>();

		public int ResultsTableId
		{
			get;
			set;
		}

		public ResultsTable ResultsTable
		{
			get;
			set;
		}
	}
}