using System;

namespace RightMove.Desktop.Services
{
	public interface ISampleService
	{
		string GetCurrentDate();
	}

	public class SampleService : ISampleService
	{
		public string GetCurrentDate() => DateTime.Now.ToLongDateString();
	}
}
