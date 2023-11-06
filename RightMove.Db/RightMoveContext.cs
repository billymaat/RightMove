using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using RightMove.Db.Entities;

namespace RightMove.Db
{
	public class RightMoveContext : DbContext
	{
		public DbSet<ResultsTable> ResultsTable { get; set; }
		public DbSet<RightMovePropertyEntity> Properties { get; set; }

		private const string ConnectionString =
			"Server=(localdb)\\mssqllocaldb;Database=RightMove;Trusted_Connection=True;MultipleActiveResultSets=true";
		public RightMoveContext()
		{
		}

		// The following configures EF to create a Sqlite database file in the
		// special "local" folder for your platform.
		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			base.OnConfiguring(options);
			options.UseSqlServer(ConnectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Configure the value converter for the RightMoveProperty
			modelBuilder.Entity<RightMovePropertyEntity>()
				.Property(x => x.Dates)
					.HasConversion(new ValueConverter<List<DateTime>, string>(
						v => JsonConvert.SerializeObject(v), // Convert to string for persistence
						v => JsonConvert.DeserializeObject<List<DateTime>>(v))); // Convert to List<String> for use

			modelBuilder.Entity<RightMovePropertyEntity>()
				.Property(x => x.Prices)
								.HasConversion(new ValueConverter<List<int>, string>(
									v => JsonConvert.SerializeObject(v), // Convert to string for persistence
									v => JsonConvert.DeserializeObject<List<int>>(v)));

			modelBuilder.Entity<ResultsTable>()
				.HasMany(c => c.Properties)
				.WithOne(e => e.ResultsTable);
		}
	}
}