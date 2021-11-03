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
		// public static string DbFile => @"C:\RightMoveDB\RightMoveDB.db";
		public static readonly string DbFile;

		static SqLiteBaseRepository()
		{
			var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

			DbFile = Path.Combine(path, "RightMoveDB.db");
		}

		public static SQLiteConnection SimpleDbConnection()
		{
			return new SQLiteConnection($"DataSource= {DbFile}");
		}
	}
}
