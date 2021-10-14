using System;

namespace RightMoveConsole.Services
{
	public class DisplayService : IDisplayService
	{
		public void WriteLine(string s)
		{
			Console.WriteLine(s);
		}

		public void WriteLine()
		{
			Console.WriteLine();
		}
	}
}
