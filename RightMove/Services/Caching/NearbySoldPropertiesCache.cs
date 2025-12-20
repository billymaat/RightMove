using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using RightMove.DataTypes;

namespace RightMove.Services.Caching
{
	public class NearbySoldPropertiesCache
	{
		private readonly IMemoryCache _memoryCache;

		public NearbySoldPropertiesCache(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public bool TryGetValue(object key, out List<NearbySoldProperty> value)
		{
			value = null;

			if (!_memoryCache.TryGetValue(key, out var rmp))
			{
				return false;
			}

			if (rmp is List<NearbySoldProperty> p)
			{
				value = p;
				return true;
			}

			return false;
		}

		public void CreateEntry(object key, List<NearbySoldProperty> value)
		{
			_memoryCache.Set(key, value);
		}

		public void Remove(object key)
		{
			_memoryCache.Remove(key);
		}
	}
}
