using System.Collections;
using System.Collections.Generic;

namespace RightMove.DataTypes
{
	public class RightMoveSearchPage : IEnumerable<RightMoveProperty>
	{
		/// <summary>
		/// Gets the list of <see cref="RightMoveProperty"/> on the page
		/// </summary>
		public RightMoveSearchItemCollection RightMoveSearchItems
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the current page
		/// </summary>
		public int CurrentPage
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the page count
		/// </summary>
		public int PageCount
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the results count
		/// </summary>
		public int ResultsCount
		{
			get;
			set;
		}

		public string SearchUrl
		{
			get;
			set;
		}

		public IEnumerator<RightMoveProperty> GetEnumerator()
		{
			return RightMoveSearchItems.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return RightMoveSearchItems.GetEnumerator();
		}
	}
}
