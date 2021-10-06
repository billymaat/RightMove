using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AngleSharp.Common;
using Newtonsoft.Json;
using Utilities;

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

		public static StringTrieSet RegionTree
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
					OutcodeDictionary.Add((string) g.outcode, (int) g.code);
				}

				// load region dictionary
				RegionDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(Properties.Resources.Regions);
				RegionTree = new StringTrieSet(new IgnoreCase());
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
