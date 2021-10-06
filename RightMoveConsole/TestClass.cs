using System;
using System.Collections.Generic;
using System.Text;

namespace RightMoveConsole
{
	public class TestClass
	{
		private List<string> BunchOfStrings = new List<string>();

		public TestClass()
		{

		}

		public void Run()
		{
			for (int i = 0; i < 100000; i++)
			{
				object obj = new Double();
				try
				{
					int k = (int)obj;
				}
				catch (Exception e)
				{

				}
			}
		}

	}
}
