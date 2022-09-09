using Microsoft.EntityFrameworkCore;
using RightMove.Db.Models;

namespace RightMove.EF
{
	public class RightMoveContext : DbContext
	{
		public DbSet<RightMovePropertyModel> AshtonUnderLyneGreaterManchester { get; set; }

		public string DbPath { get; }

		public RightMoveContext()
		{
			var folder = Environment.SpecialFolder.LocalApplicationData;
			var path = Environment.GetFolderPath(folder);
			DbPath = System.IO.Path.Join(path, "rightmove.db");
			DbPath = @"C:\cygwin64\home\Billy\RightMoveDB.db";
		}

		// The following configures EF to create a Sqlite database file in the
		// special "local" folder for your platform.
		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite($"Data Source={DbPath}");
	}
}