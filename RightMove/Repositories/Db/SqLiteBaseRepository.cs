using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace RightMove.Repositories.Db
{
	public class SqLiteBaseRepository
	{
		public static string DbFile => @"C:\RightMoveDB\RightMoveDB.db";

		public static SQLiteConnection SimpleDbConnection()
		{
			return new SQLiteConnection($"DataSource= {DbFile}");
		}
	}
}
