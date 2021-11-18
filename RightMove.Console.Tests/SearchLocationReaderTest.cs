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
		public void SearchLocationsReader()
		{
			SearchLocationsReader reader = new SearchLocationsReader(() => @"C:\Users\Billy\source\repos\RightMove\RightMoveConsole\searchlocations.txt");
			var filenames = reader.GetLocations();
			Assert.IsNotNull(filenames);
			Assert.IsTrue(filenames.Count > 0);
		}

		[Test]
		public void SearchLocationReader_NullFilePath()
		{
			SearchLocationsReader reader = new SearchLocationsReader(null);
			var filenames = reader.GetLocations();
			Assert.IsNull(filenames);
		}
	}
}