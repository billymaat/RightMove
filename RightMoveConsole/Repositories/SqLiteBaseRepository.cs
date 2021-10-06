using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace RightMoveConsole.Repositories
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
