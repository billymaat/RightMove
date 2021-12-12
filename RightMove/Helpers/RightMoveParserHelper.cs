using System;
using System.Globalization;
using System.Text.RegularExpressions;
using RightMove.Extensions;

namespace RightMove.Helpers
{
	public static class RightMoveParserHelper
	{
		/// <summary>
		/// Parse the date reduced 
		/// </summary>
		/// <param name="dateText">the text</param>
		/// <returns>the date reduced, or <see cref="DateTime.MinValue"/> if unsuccessful</returns>
		public static DateTime ParseDateReduced(string dateText)
		{
			DateTime date = DateTime.MinValue;
			string reduced = "Reduced";
			int ind = dateText.IndexOf(reduced, StringComparison.CurrentCultureIgnoreCase);

			if (ind >= 0)
			{
				// we might fail to get a string, but it doesn't matter
				var dateString = dateText.Substring(ind + "Reduced on ".Length, 10);

				if (!DateTime.TryParse(dateString, out date))
				{
					date = ParseDate(dateText);
				}
			}

			return date;
		}

		/// <summary>
		/// Parse date added
		/// </summary>
		/// <param name="dateText">the text to parse</param>
		/// <returns>the date added, or <see cref="DateTime.MinValue"/></returns>
		public static DateTime ParseDateAdded(string dateText)
		{
			DateTime date = DateTime.MinValue;

			string added = "Added";

			int ind = dateText.IndexOf(added, StringComparison.CurrentCultureIgnoreCase);

			if (ind >= 0)
			{
				var dateString = dateText.Substring(ind + "Added on ".Length, 10);
			
				if (!DateTime.TryParse(dateString, out date))
				{
					date = ParseDate(dateText);
				}
			}

			return date;
		}

		/// <summary>
		/// Parse date from "yesterday" or "today"
		/// </summary>
		/// <param name="dateText">the text</param>
		/// <returns>the date, or <see cref="DateTime.MinValue"/></returns>
		private static DateTime ParseDate(string dateText)
		{
			DateTime date = DateTime.MinValue;

			string yesterday = "yesterday";
			string today = "today";

			if (dateText.IndexOf(yesterday, StringComparison.InvariantCultureIgnoreCase) >= 0)
			{
				date = DateTime.Now.AddDays(-1);
			}
			else if (dateText.IndexOf(today, StringComparison.InvariantCultureIgnoreCase) >= 0)
			{
				date = DateTime.Now;
			}

			return date;
		}

		/// <summary>
		/// Parse the agent
		/// </summary>
		/// <param name="agentText">the text</param>
		/// <returns>the agent, or null if unsuccessful</returns>
		public static string ParseAgent(string agentText)
		{
			string agent = null;
			int ind = agentText.IndexOf("by ", StringComparison.CurrentCultureIgnoreCase);
			if (ind >= 0)
			{
				agent = agentText.Substring(ind + 3);
				agent = agent.TrimUp();
			}

			return agent;
		}

		/// <summary>
		/// Parse the price
		/// </summary>
		/// <param name="priceText">the price as text</param>
		/// <returns>the price, or -1 if unccessful</returns>
		public static int ParsePrice(string priceText)
		{
			Regex reg = new Regex(@"[0-9,]+");
			var match = reg.Match(priceText);

			if (!match.Success || !int.TryParse(match.Value, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int price))
			{
				price = -1;
			}

			return price;
		}
	}
}
