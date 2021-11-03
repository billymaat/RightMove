namespace RightMove.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Trim a string of \r, \n and \t
		/// </summary>
		/// <param name="str">The <see cref="string"/> to trim</param>
		/// <returns>The trimmed string</returns>
		public static string TrimUp(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}

			var ret = str.Trim('\r', '\n', '\t');

			return ret.Trim();
		}
	}
}
