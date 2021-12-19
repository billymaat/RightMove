using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Text;

namespace RightMove.Db.Repositories
{
	public class SqLiteBaseRepository
	{		
		public SqLiteBaseRepository(IDbConfiguration dbConfiguration)
		{
			DbConfiguration = dbConfiguration;
		}

		public IDbConfiguration DbConfiguration
		{
			get;
		}

		public string DbFile
		{
			get
			{
				var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				return Path.Combine(path, DbConfiguration.DbFile);
			}
		}

		public SQLiteConnection SimpleDbConnection()
		{
			return new SQLiteConnection($"DataSource= {DbFile}");
		}
	}
}
