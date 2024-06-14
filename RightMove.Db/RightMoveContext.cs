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
		public DbSet<DatePrice> Prices { get; set; }

		public RightMoveContext(DbContextOptions<RightMoveContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ResultsTable>()
				.HasMany(c => c.Properties)
				.WithOne(e => e.ResultsTable);
		}
	}
}