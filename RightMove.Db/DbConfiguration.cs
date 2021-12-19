using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove.Db
{
	public class DbConfiguration : IDbConfiguration
	{
		public DbConfiguration(string dbFile)
		{
			DbFile = dbFile;
		}

		public string DbFile
		{
			get;
		}
	}
}
