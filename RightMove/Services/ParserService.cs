using System;
using System.Globalization;
using System.Text.RegularExpressions;
using RightMove.Extensions;

namespace RightMove.Services
{
	public static class RightMoveParserHelper
	{
		public static DateTime ParseDateReduced(string dateText)
		{
			DateTime date = DateTime.MinValue;
			string reducedOn = "Reduced on ";
			int ind = dateText.IndexOf(reducedOn, StringComparison.CurrentCultureIgnoreCase);
			int length = reducedOn.Length;

			if (ind >= 0)
			{
				// we might fail to get a string, but it doesn't matter
				var dateString = dateText.Substring(ind + length, 10);
				DateTime.TryParse(dateString, out date);
			}

			return date;
		}



		public static DateTime ParseDateAdded(string dateText)
		{
			DateTime date = DateTime.MinValue;

			string addedOn = "Added on ";
			int ind = dateText.IndexOf(addedOn, StringComparison.CurrentCultureIgnoreCase);
			int length = addedOn.Length;

			if (ind >= 0)
			{
				// we might fail to get a string, but it doesn't matter
				var dateString = dateText.Substring(ind + length, 10);
				DateTime.TryParse(dateString, out date);
			}

			return date;
		}

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
