using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io.Network;

namespace RightMove.Services
{
	public interface IHttpService
	{
		/// <summary>
		/// Get a Document from a url
		/// </summary>
		/// <param name="url">the url</param>
		/// <returns>Returns the <see cref="IDocument"/></returns>
		Task<IDocument> GetDocument(string url, CancellationToken cancellationToken = default(CancellationToken));

		byte[] DownloadImage(string uri, CancellationToken cancellationToken = default(CancellationToken));
		Task<byte[]> DownloadImageAsync(string uri, CancellationToken cancellationToken = default(CancellationToken));
	}

	public class HttpService : IHttpService
	{
		/// <summary>
		/// Get a Document from a url
		/// </summary>
		/// <param name="url">the url</param>
		/// <returns>Returns the <see cref="IDocument"/></returns>
		public async Task<IDocument> GetDocument(string url, CancellationToken cancellationToken = default(CancellationToken))
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:69.0) Gecko/20100101 Firefox/69.0");

			var requester = new HttpClientRequester(client);

			// var config = Configuration.Default.WithDefaultLoader().WithDefaultCookies();
			var config = Configuration.Default.WithRequester(requester).WithDefaultLoader().WithDefaultCookies();
			var context = BrowsingContext.New(config);

			IDocument document = null;

			try
			{
				document = await context.OpenAsync(url, cancellationToken).ConfigureAwait(false);
			}
			catch (Exception ex)
			{

			}
			return document;
		}

		public byte[] DownloadImage(string uri, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (WebClient client = new WebClient())
			{
				// client.DownloadDataAsync(uri, cancellationToken);
				byte[] pic = client.DownloadData(uri);
				return pic;
			}
		}

		//public Task<byte[]> DownloadImageAsync(string uri, CancellationToken cancellationToken = default(CancellationToken))
		//{
		//	using (WebClient client = new WebClient())
		//	{
		//		byte[] data = null;
		//		client.DownloadDataCompleted += async delegate (object sender, DownloadDataCompletedEventArgs e)
		//		{
		//			data = e.Result;
		//		};

		//		client.DownloadDataAsync(new Uri(uri), cancellationToken);
		//		while (client.IsBusy)
		//		{
		//			Task.Delay(100, cancellationToken);
		//		}

		//		return data;
		//	}
		//}

		public async Task<byte[]> DownloadImageAsync(string uri, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (WebClient client = new WebClient())
			{
				byte[] data = await client.DownloadDataTaskAsync(new Uri(uri));
				return data;
			}
		}
	}
}
