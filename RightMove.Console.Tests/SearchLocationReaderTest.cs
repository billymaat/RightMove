using Moq;
using NUnit.Framework;
using RightMoveConsole.Services;

namespace RightMove.Console.Tests
{
	public class SearchLocationReaderTest
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void SearchLocationsReader ()
		{
			SearchLocationsReader reader = new SearchLocationsReader(() => @"C:\Users\Billy\source\repos\RightMove\RightMoveConsole\searchlocations.txt");
			var filenames = reader.GetFilenames();
			Assert.IsNotNull(filenames);
			Assert.IsTrue(filenames.Count > 0);
		}
	}
}