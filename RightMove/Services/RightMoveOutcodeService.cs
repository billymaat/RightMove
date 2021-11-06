using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.XPath;
using Newtonsoft.Json;
using RightMove.DataTypes;

namespace RightMove.Services
{
	/// <summary>
	/// We need the outcode for all postcodes. This class tries to try different outcodes, which are integer values, to try and figure
	/// out what post code matches what outcode.
	/// This definitely needs tidying up
	/// </summary>
	public class RightMoveOutcodeService
	{
		private static readonly BlockingCollection<string> m_Queue = new BlockingCollection<string>();

		public RightMoveOutcodeService()
		{
			var thread = new Thread(
				() =>
				{
					while (true) Console.WriteLine(m_Queue.Take());
				});
			thread.IsBackground = true;
			thread.Start();
		}


		public async Task<Dictionary<string, string>> DoSearchAsync()
		{
			Dictionary<string, string> outcodeDictionary = new Dictionary<string, string>();

			for (int i = 1; i < 5; i++)
			{
				var location = string.Format("OUTCODE^{0}", i);
				SearchParams searchParams = new SearchParams()
				{
					OutcodeLocation = location
				};

				string searchUrl = string.Format("{0}?{1}", RightMoveUrls.SearchUrl, searchParams.EncodeOptions());

				var outcode = GetOutcodeFromDocumentAsync(searchUrl);

				if (string.IsNullOrEmpty(await outcode))
				{
					break;
				}
				outcodeDictionary.Add(i.ToString(), await outcode);

				WriteLine($"{i.ToString()} : {await outcode}");
			}

			using (StreamWriter s = new StreamWriter("outcodes.json"))
			{
				var json = JsonConvert.SerializeObject(outcodeDictionary);
				await s.WriteLineAsync(json);
			}

			WriteLine("Finished");

			return outcodeDictionary;
		}

		public static void WriteLine(string value)
		{
			m_Queue.Add(value);
		}

		private async Task<string> GetOutcodeFromDocumentAsync(string searchUrl)
		{
			var document = await GetDocument(searchUrl);
			var search = document.Body.SelectSingleNode("//*[@id='searchTitle']/h1");

			if (search is null || string.IsNullOrEmpty(search.TextContent))
			{
				return null;
			}

			string searchString = "Properties For Sale in ";
			int index = search.TextContent.IndexOf(searchString, StringComparison.CurrentCulture);
			if (index < 0)
			{
				return null;
			}

			string outcode = search.TextContent.Substring(searchString.Length);
			return outcode;
		}

		private async Task<IDocument> GetDocument(string searchUrl)
		{
			var config = Configuration.Default.WithDefaultLoader();
			var context = BrowsingContext.New(config);
			var document = await context.OpenAsync(searchUrl);
			return document;
		}
	}
}
