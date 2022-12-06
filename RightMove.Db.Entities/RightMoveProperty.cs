namespace RightMove.Db.Entities
{
	public class RightMoveProperty
	{
		public RightMoveProperty()
		{
		}

		public RightMoveProperty(RightMoveProperty rightMoveProperty)
		{
			RightMoveId = rightMoveProperty.RightMoveId;
			HouseInfo = rightMoveProperty.HouseInfo;
			Address = rightMoveProperty.Address;

			DateAdded = rightMoveProperty.DateAdded;
			DateReduced = rightMoveProperty.DateReduced;
			Date = DateTime.Now;
		}

		public int Id
		{
			get;
			set;
		}

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

		public List<int> Prices
		{
			get;
			set;
		} = new List<int>();

		public List<DateTime> Dates
		{
			get;
			set;
		} = new List<DateTime>();
	}
}