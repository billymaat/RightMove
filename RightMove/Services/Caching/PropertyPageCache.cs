using Microsoft.Extensions.Caching.Memory;
using RightMove.DataTypes;

namespace RightMove.Services.Caching
{
	public class PropertyPageCache
	{
		private readonly IMemoryCache _memoryCache;
		public PropertyPageCache(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public bool TryGetValue(object key, out RightMoveProperty value)
		{
			value = null;

			if (!_memoryCache.TryGetValue(key, out var rmp))
			{
				return false;
			}

			if (rmp is RightMoveProperty p)
			{
				value = p;
				return true;
			}

			return false;
		}

		public void CreateEntry(object key, RightMoveProperty value)
		{
			_memoryCache.Set(key, value);
		}

		public void Remove(object key)
		{
			_memoryCache.Remove(key);
		}
	}
}
