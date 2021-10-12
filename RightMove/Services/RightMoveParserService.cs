using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AngleSharp.Dom;
using RightMove.DataTypes;
using RightMove.Factory;

namespace RightMove.Services
{
	public class RightMoveParserService
	{
		public const int PriceNotSet = -1;

		private readonly IHttpService _httpService;
		private readonly SearchPageParserServiceFactory _searchPageParseFactory;
		private readonly ILoggerService _logger;
		private readonly string _searchUrl;

		/// <summary>
		/// Initializes a new instance <see cref="RightMoveParserService"/> class
		/// </summary>
		/// <param name="httpService">the http service</param>
		/// <param name="searchPageParseFactory">the <see cref="SearchPageParserServiceFactory"> service</param>
		/// <param name="searchParams">the <see cref="SearchParams"/></param>
		public RightMoveParserService(IHttpService httpService, 
			SearchPageParserServiceFactory searchPageParseFactory,
			ILoggerService logger,
			SearchParams searchParams)
		{
			_httpService = httpService;
			_searchPageParseFactory = searchPageParseFactory;
			_logger = logger;

			if (searchParams is null)
			{
				throw new ArgumentNullException($"{nameof(searchParams)} must not be null");
			}

			SearchParams = searchParams;
			
			// construct the search url from the SearchParams
			_searchUrl = $"{RightMoveUrls.SearchUrl}?{SearchParams.EncodeOptions()}";
		}

		/// <summary>
		/// Gets the <see cref="SearchParams"/>
		/// </summary>
		public SearchParams SearchParams
		{
			get;
		}

		/// <summary>
		/// Gets the list of results
		/// </summary>
		public RightMoveSearchItemCollection Results
		{
			get;
			private set;
		}

		/// <summary>
		/// Perform a search
		/// </summary>
		/// <returns>true if successful, false otherwise</returns>
		public async Task<bool> SearchAsync()
		{
			int pageCount = 1;

			// first get the page count (one redundant search I guess)
			var searchPageService = _searchPageParseFactory.CreateInstance();

			RightMoveSearchPage rightMoveFirstPageSearch = await searchPageService
				.ParseRightMoveSearchPageAsync(_searchUrl).ConfigureAwait(false);

			if (rightMoveFirstPageSearch is null)
			{
				_logger?.WriteLine("First search page is null");
				return false;
			}

			pageCount = GetPageCount(rightMoveFirstPageSearch, pageCount);

			_logger?.WriteLine($"Page count: {pageCount}");
			
			// extract the items
			var listOfTasks = ExtractRightMoveItems(pageCount);

			RightMoveSearchItemCollection rightMoveItems = new RightMoveSearchItemCollection();
			
			// wait until we've got all the results, and then add them all to a list
			await Task.WhenAll(listOfTasks).ConfigureAwait(false);
			foreach (var task in listOfTasks)
			{
				if (task.Result != null)
				{
					if (task.Result.RightMoveSearchItems.Count > 0)
					{
						rightMoveItems.AddRangeUnique(task.Result.RightMoveSearchItems);
					}
					else
					{
						
					}

				}
				else
				{

				}
			}
			
			Results = rightMoveItems;

			return true;
		}

		private int GetPageCount(RightMoveSearchPage rightMoveFirstPageSearch, int pageCount)
		{
			if (rightMoveFirstPageSearch.ResultsCount >= 0)
			{
				pageCount = (int) Math.Ceiling(rightMoveFirstPageSearch.ResultsCount / 24.0);
			}

			// check the page count
			if (pageCount > 42)
			{
				_logger?.WriteLine("Fixed pagecount to 42");
				pageCount = 42;
			}

			return pageCount;
		}

		private List<Task<RightMoveSearchPage>> ExtractRightMoveItems(int pageCount)
		{
			List<Task<RightMoveSearchPage>> listOfTasks = new List<Task<RightMoveSearchPage>>(pageCount);

			// the multiple for the page number
			int multiple = 24;
			for (int i = 1; i <= pageCount; i++)
			{
				int index = (i - 1) * multiple;
				var searchUrlWithPageNumber = _searchUrl + "&index=" + index;
				var task = GetSearchPages(searchUrlWithPageNumber);
				listOfTasks.Add(task);
			}

			return listOfTasks;
		}

		private async Task<RightMoveSearchItemCollection> GetSearchResults(string searchUrlWithPageNumber)
		{
			RightMoveSearchPage rightMovePage = await GetSearchPages(searchUrlWithPageNumber).ConfigureAwait(false);

			if (rightMovePage is null)
			{
				_logger?.WriteLine($"{nameof(rightMovePage)} was null");
				return null;
			}

			if (rightMovePage.RightMoveSearchItems is null)
			{
				_logger?.WriteLine($"{nameof(rightMovePage.RightMoveSearchItems)} was null");
				return null;
			}

			return rightMovePage.RightMoveSearchItems;
		}

		private async Task<RightMoveSearchPage> GetSearchPages(string searchUrlWithPageNumber)
		{
			// parse the search page
			var searchPageService = _searchPageParseFactory.CreateInstance();
			return await searchPageService.ParseRightMoveSearchPageAsync(searchUrlWithPageNumber).ConfigureAwait(false);
		}
	}
}
