using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove
{
	internal class RightMoveUrls
	{
		public const string RightMoveUrl = @"https://www.rightmove.co.uk";
		public const string SearchUrl = @"https://www.rightmove.co.uk/property-for-sale/find.html";
		public const string PropertyUrl = @"https://www.rightmove.co.uk/properties/";

		public static string GetPropertyUrl(int propertyId)
		{
			string url = $"{PropertyUrl}/{propertyId}";
			return url;
		}
	}
}
