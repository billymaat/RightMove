using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RightMove.Services;

namespace RightMove.DataTypes
{
	/// <summary>
	/// A collection of right move items
	/// </summary>
	public class RightMoveSearchItemCollection : IList<RightMoveProperty>
	{
		private int _id = 0;

		private readonly List<RightMoveProperty> _lst;

		public RightMoveSearchItemCollection()
		{
			_lst = new List<RightMoveProperty>();
		}

		public RightMoveSearchItemCollection(int n)
		{
			_lst = new List<RightMoveProperty>(n);
		}

		public RightMoveSearchItemCollection(IEnumerable<RightMoveProperty> lst)
		{
			_lst = new List<RightMoveProperty>(lst);
		}

		public double AveragePrice => CalculateAveragePrice();

		public double CalculateAveragePrice()
		{
			return this.Where(o => o.Price != RightMoveParserService.PriceNotSet)
				.Select(o => o.Price)
				.Average();
		}

		public void AddUnique(RightMoveProperty item)
		{
			if (!ContainsPropertyId(item.RightMoveId))
			{
				Add(item);
			}
		}

		public void Add(RightMoveProperty item)
		{
			_lst.Add(item);
			item.Id = GetNextId();
		}

		public void Clear()
		{
			_lst.Clear();
			// throw new NotImplementedException();
		}

		public bool Contains(RightMoveProperty item)
		{
			return _lst.Contains(item);
		}

		public void CopyTo(RightMoveProperty[] array, int arrayIndex)
		{
			_lst.CopyTo(array, arrayIndex);
		}

		public bool Remove(RightMoveProperty item)
		{
			return _lst.Remove(item);
		}

		public int Count => _lst.Count;

		public bool IsReadOnly => false;

		public void AddRangeUnique(RightMoveSearchItemCollection items)
		{
			foreach (var rightMoveSearchItem in items)
			{
				AddUnique(rightMoveSearchItem);
			}
		}

		public void AddRange(RightMoveSearchItemCollection items)
		{
			throw new NotImplementedException("we do not add non unique ranges");
		}

		public IEnumerator<RightMoveProperty> GetEnumerator()
		{
			return _lst.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private int GetNextId()
		{
			return ++_id;
		}

		public int IndexOf(RightMoveProperty item)
		{
			return _lst.IndexOf(item);
		}

		public void Insert(int index, RightMoveProperty item)
		{
			_lst.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_lst.RemoveAt(index);
		}

		public RightMoveProperty this[int index]
		{
			get => _lst[index];
			set => _lst[index] = value;
		}

		private bool ContainsPropertyId(int id)
		{
			if (this.Any(o => o.RightMoveId == id))
			{
				return true;
			}

			return false;
		}
	}
}
