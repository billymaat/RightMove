using System;
using System.Globalization;
using System.Text.RegularExpressions;
using RightMove.Extensions;

namespace RightMove.Services
{
	public static class RightMoveParserHelper
	{
		public static DateTime ParseDate(string dateText)
		{
			DateTime date = DateTime.MinValue;

			string addedOn = "Added on ";
			string reducedOn = "Reduced on ";
			int ind = dateText.IndexOf(addedOn, StringComparison.CurrentCultureIgnoreCase);
			int length = addedOn.Length;

			if (ind < 0)
			{
				ind = dateText.IndexOf(reducedOn, StringComparison.CurrentCultureIgnoreCase);
				length = reducedOn.Length;
			}

			if (ind >= 0)
			{
				var dateString = dateText.Substring(ind + length, 10);
				if (!DateTime.TryParse(dateString, out date))
				{

				}
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
