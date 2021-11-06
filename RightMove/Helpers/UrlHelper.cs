using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RightMove.Helpers
{
	public class UrlHelper
	{
		/// <summary>
		/// Convert a dictionary of options to url encoded string
		/// </summary>
		/// <param name="options">The options as a <see cref="Dictionary{TKey,TValue}"/></param>
		/// <returns>The encoded options as string</returns>
		public static string EncodeParameters(Dictionary<string, string> options)
		{
			var optionsString = string.Join("&", options.Select(o => HttpUtility.UrlEncode((string)o.Key) + "=" + HttpUtility.UrlEncode((string)o.Value)));

			return optionsString;
		}
	}
}
