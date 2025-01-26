namespace RightMove.Desktop.Helpers
{
	public static class BrowserHelper
	{
		/// <summary>
		/// Open browser to <see cref="url"/>
		/// </summary>
		/// <param name="url">the url</param>
		public static void OpenWebpage(string url)
		{
			var sInfo = new System.Diagnostics.ProcessStartInfo(url)
			{
				UseShellExecute = true,
			};

			System.Diagnostics.Process.Start(sInfo);
		}
	}
}
