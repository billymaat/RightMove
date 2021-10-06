using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace RightMove.Services
{
	public interface ILoggerService
	{
		void WriteLine(string str);
	}

	public class LoggerService : ILoggerService
	{
		public void WriteLine(string str)
		{
			Console.WriteLine(str);
		}
	}
}
