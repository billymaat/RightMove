using System;
using System.Collections.Generic;
using System.Text;

namespace RightMoveApp.Services
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
