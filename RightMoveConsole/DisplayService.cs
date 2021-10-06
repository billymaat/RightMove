using System;
using System.Collections.Generic;
using System.Text;
using RightMove.Services;

namespace RightMoveConsole
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
