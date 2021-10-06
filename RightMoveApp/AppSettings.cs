using System;
using System.Collections.Generic;
using System.Text;

namespace RightMoveApp
{
	public class AppSettings
	{
		public string StringSetting { get; set; }

		public int IntegerSetting { get; set; }

		public bool BooleanSetting { get; set; }
		
		public string ConnectionString { get; set; }
		
		public string ProviderName { get; set; }
	}
}
