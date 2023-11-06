using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using RightMove.DataTypes;
using RightMove.Extensions;
using RightMove.Factory;
using RightMove.Helpers;

namespace RightMove.Services
{
	public class SearchPageParserService
	{
		/// <summary>
		/// Class of selectors
		/// </summary>
		private static class Selector
		{
			public const string ResultsCount = "#searchHeader > span";
			public const string Link = "div > div > div.propertyCard-content > div.propertyCard-section > div.propertyCard-details > a";
			public const string HouseType = "div > div > div.propertyCard-content > div.propertyCard-section > div.propertyCard-details > a > h2";
			public const string Address = "div > div > div.propertyCard-content > div.propertyCard-section > div.propertyCard-details > a > address";
			public const string DateAndAgent = "div > div > div.propertyCard-content > div.propertyCard-detailsFooter > div.propertyCard-branchSummary";
			public const string Desc = "div > div > div.propertyCard-content > div.propertyCard-section > div.propertyCard-description";
			public const string Price = "div > div > div.propertyCard-header > div > a > div.propertyCard-priceValue";
			public const string Featured = "div > div > div.propertyCard-moreInfo > div.propertyCard-moreInfoFeaturedTitle";

			// these two page selectors will never work as this code is hidden behind a knockout JS 'component'
			public const string PageCount = "#l-container > div.l-propertySearch-paginationWrapper > div > div > div > div.pagination-pageSelect > span:nth-child(4)";
			public const string CurrentPage = "#l-container > div.l-propertySearch-paginationWrapper > div > div > div > div.pagination-pageSelect > div";
		}

		private readonly RightMovePropertyFactory _propertyFactory;
		private readonly IHttpService _httpService;
		private readonly ILogger _logger;

		public SearchPageParserService(
			IHttpService httpService,
			ILogger logger,
			RightMovePropertyFactory propertyFactory)
		{
			_httpService = httpService;
			_logger = logger;
			_propertyFactory = propertyFactory;
		}

		public RightMoveSearchPage Page
		{
			get;
			private set;
		}

		public IDocument Document
		{
			get;
			set;
		}

		public string SearchUrl
		{
			get;
			private set;
		}

		private void ParseRightMoveSearchPage()
		{
			var pageText = Document.Body.QuerySelector(Selector.PageCount)?.Text();

			int page;
			if (!int.TryParse(pageText, out page))
			{
				page = -1;
			}

			var resultCountText = Document.Body.QuerySelector(Selector.ResultsCount)?.Text();

			if (!int.TryParse(resultCountText, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int resultsCount))
			{
				resultsCount = -1;
			}

			var currentPageText = Document.QuerySelector(Selector.CurrentPage)?.Text();
			if (!int.TryParse(currentPageText, out int currentPage))
			{
				currentPage = -1;
			}

			var propertyNodes = Document.All.Where(m => !string.IsNullOrEmpty(m.Id) && m.Id.StartsWith("property-"));

			RightMoveSearchItemCollection rightMoveItems = new RightMoveSearchItemCollection();

			foreach (var propertyNode in propertyNodes)
			{
				string propertyCardPrefix = "property-";
				int rightMoveId;
				if (!int.TryParse(propertyNode.Id.Substring(propertyCardPrefix.Length), out rightMoveId) || rightMoveId == 0)
				{
					continue;
				}

				// properties
				string houseType;
				string address;
				string desc;
				DateTime dateAdded = DateTime.MinValue;
				DateTime dateReduced = DateTime.MinValue;
				string agent = null;
				string link;
				int price;
				bool featured = false;

				houseType = propertyNode.QuerySelector(Selector.HouseType)?.Text();
				houseType = houseType?.TrimUp();

				link = propertyNode.QuerySelector(Selector.Link).GetAttribute("href");
				address = propertyNode.QuerySelector(Selector.Address)?.Text();
				address = address?.TrimUp();

				desc = propertyNode.QuerySelector(Selector.Desc)?.Text();
				desc = address?.TrimUp();

				var dateAndAgentText = propertyNode.QuerySelector(Selector.DateAndAgent)?.Text();

				// dateAndAgent is in the form "Added on 01/02/2021 by Melissa Berry Sales & Lettings, Prestwich"
				if (!string.IsNullOrEmpty(dateAndAgentText))
				{
					dateAdded = RightMoveParserHelper.ParseDateAdded(dateAndAgentText);
					dateReduced = RightMoveParserHelper.ParseDateReduced(dateAndAgentText);
					agent = RightMoveParserHelper.ParseAgent(dateAndAgentText);
				}

				var priceText = propertyNode.QuerySelector(Selector.Price)?.Text();

				price = RightMoveParserHelper.ParsePrice(priceText);

				var featuredText = propertyNode.QuerySelector(Selector.Featured)?.Text();
				if (featuredText?.IndexOf("featured", StringComparison.CurrentCultureIgnoreCase) >= 0)
				{
					featured = true;
				}

				RightMoveProperty rightMoveItem = _propertyFactory.Create();
				rightMoveItem.RightMoveId = rightMoveId;
				rightMoveItem.HouseInfo = houseType;
				rightMoveItem.Address = address;
				rightMoveItem.Desc = desc;
				rightMoveItem.Agent = agent;
				rightMoveItem.DateAdded = dateAdded;
				rightMoveItem.DateReduced = dateReduced;
				rightMoveItem.Link = link;
				rightMoveItem.Price = price;
				rightMoveItem.Featured = featured;


				rightMoveItems.Add(rightMoveItem);
			}

			RightMoveSearchPage rightMovePage = new RightMoveSearchPage()
			{
				RightMoveSearchItems = new RightMoveSearchItemCollection(rightMoveItems),
				PageCount = page,
				CurrentPage = currentPage,
				ResultsCount = resultsCount
			};

			Page = rightMovePage;
		}

		/// <summary>
		/// ParseDate a right move search page
		/// </summary>
		/// <param name="searchUrl"></param>
		/// <returns>returns <see cref="RightMoveSearchPage"/> is successful, or null otherwise</returns>
		public async Task<RightMoveSearchPage> ParseRightMoveSearchPageAsync(string searchUrl)
		{
			SearchUrl = searchUrl;
			var document = await _httpService.GetDocument(searchUrl).ConfigureAwait(false);
			var page = ParseRightMoveSearchPage(document);
			page.SearchUrl = searchUrl;
			return page;
		}

		private RightMoveSearchPage ParseRightMoveSearchPage(IDocument document)
		{
			if (document is null)
			{
				_logger.LogError($"{nameof(document)} was null");
				return null;
			}

			Document = document;
			ParseRightMoveSearchPage();
			return Page;
		}
	}
}
