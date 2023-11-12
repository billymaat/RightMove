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
		private readonly string _connectionString;
		public DbSet<ResultsTable> ResultsTable { get; set; }
		public DbSet<RightMovePropertyEntity> Properties { get; set; }
		
		public RightMoveContext(string connectionString)
		{
			_connectionString = connectionString;
			//_connectionString = "Server=(localdb)\\mssqllocaldb;Database=RightMove;Trusted_Connection=True;MultipleActiveResultSets=true";
		}

		public RightMoveContext(DbContextOptions<RightMoveContext> options) : base(options) { }

		// The following configures EF to create a Sqlite database file in the
		// special "local" folder for your platform.
		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseSqlServer(_connectionString);

			//base.OnConfiguring(options);
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