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

		public RightMoveContext(DbContextOptions<RightMoveContext> options) : base(options)
		{
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