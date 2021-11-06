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
using RightMove.Helpers;

namespace RightMove.Services
{
	/// <summary>
	/// This is a region scraper
	/// </summary>
	public class RightMoveRegionService
	{
		private static readonly BlockingCollection<string> m_Queue = new BlockingCollection<string>();

		public RightMoveRegionService()
		{
			var thread = new Thread(
				() =>
				{
					while (true) Console.WriteLine(m_Queue.Take());
				});
			thread.IsBackground = true;
			thread.Start();
		}

		public static void WriteLine(string value)
		{
			m_Queue.Add(value);
		}

		public async Task<Dictionary<string, string>> DoSearchAsync()
		{
			Dictionary<string, string> regionDictionary = new Dictionary<string, string>();

			int failedCount = 0;

			for (int i = 1; ; i++)
			{
				if (failedCount > 30)
				{
					WriteLine($"Something went wrong at i = {i}");
					break;
				}

				var location = $"REGION^{i}";

				SearchParams searchParams = new SearchParams()
				{
					OutcodeLocation = location
				};

				Dictionary<string, string> options = new Dictionary<string, string>();

				options.Add("locationIdentifier", $"REGION^{i}");
				var encodedOptions = UrlHelper.EncodeParameters(options);
				string searchUrl = $"{RightMoveUrls.SearchUrl}?{encodedOptions}";

				var outcode = GetOutcodeFromDocumentAsync(searchUrl);

				if (string.IsNullOrEmpty(await outcode))
				{
					failedCount++;
					continue;
				}

				regionDictionary.Add(i.ToString(), await outcode);
				failedCount = 0;
				WriteLine(string.Format("{0}: {1}", i.ToString(), await outcode));
			}

			using (StreamWriter s = new StreamWriter("regions.json"))
			{
				var json = JsonConvert.SerializeObject(regionDictionary);
				await s.WriteLineAsync(json);
			}

			WriteLine("Finished");

			return regionDictionary;
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
