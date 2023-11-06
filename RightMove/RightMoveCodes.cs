using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace RightMove
{
	public class RightMoveCodes
	{
		/// <summary>
		/// Gets a dictionary of the outcodes
		/// </summary>
		public static Dictionary<string, int> OutcodeDictionary
		{
			get;
			private set;
		}

		public static Dictionary<string, int> RegionDictionary
		{
			get;
			private set;
		}

		public static List<string> RegionTree
		{
			get;
			set;
		}

		static RightMoveCodes()
		{
			Action action = () =>
			{
				// load outcode dictionary
				var outcodeJson = JsonConvert.DeserializeObject<dynamic>(Properties.Resources.Outcodes);
				OutcodeDictionary = new Dictionary<string, int>();
				foreach (var g in outcodeJson)
				{
					OutcodeDictionary.Add((string)g.outcode, (int)g.code);
				}

				// load region dictionary
				RegionDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(Properties.Resources.Regions);
				RegionTree = new List<string>();
				RegionTree.AddRange(RegionDictionary.Keys);
			};

			action();
		}

		private class IgnoreCase : IEqualityComparer<char>
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
}
