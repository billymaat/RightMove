using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Utilities.Comparers
{
	public class IgnoreCaseEqualityComparer : IEqualityComparer<char>
	{
		public bool Equals([AllowNull] char x, [AllowNull] char y)
		{
			return char.Equals(char.ToUpper(x), char.ToUpper(y));
		}

		public int GetHashCode([DisallowNull] char obj)
		{
			return char.ToUpper(obj).GetHashCode();
		}
	}
}
