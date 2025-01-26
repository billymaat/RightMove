using System;
using RightMove.DataTypes;

namespace RightMove.Desktop.Model
{
	public class RightMoveViewItem
	{
		private RightMoveProperty _item;

		public RightMoveViewItem()
		{

		}
		public RightMoveViewItem(RightMoveProperty rightMoveSearchItem)
		{
			_item = rightMoveSearchItem;
		}

		public int Id
		{
			get
			{
				return _item.Id;
			}
		}

		public int RightMoveId
		{
			get
			{
				return _item.RightMoveId;
			}
		}

		public string Desc
		{
			get
			{
				return _item.Desc;
			}
		}

		public string HouseInfo
		{
			get
			{
				return _item.HouseInfo;
			}
		}

		public string Address
		{
			get
			{
				return _item.Address;
			}
		}

		public string Agent
		{
			get
			{
				return _item.Agent;
			}
		}

		public DateTime Date
		{
			get
			{
				return _item.DateAdded;
			}
		}

		public int Price
		{
			get
			{
				return _item.Price;
			}
		}

		public string Url
		{
			get { return _item.Url; }
		}

		public bool Featured
		{
			get
			{
				return _item.Featured;
			}
		}
	}
}
