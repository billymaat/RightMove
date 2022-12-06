﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using RightMove.Db.Entities;
using RightMove.Db.Models;

namespace RightMove.EF
{
	public class RightMoveContext : DbContext
	{
		public DbSet<ResultsTable> ResultsTable { get; set; }
		public DbSet<RightMoveProperty> Properties { get; set; }

		public string DbPath { get; }

		public RightMoveContext()
		{
			var folder = Environment.SpecialFolder.LocalApplicationData;
			var path = Environment.GetFolderPath(folder);
			DbPath = System.IO.Path.Join(path, "rightmove.db");
			//DbPath = @"C:\cygwin64\home\Billy\RightMoveDB.db";
		}

		// The following configures EF to create a Sqlite database file in the
		// special "local" folder for your platform.
		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite($"Data Source={DbPath}");

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Configure the value converter for the Animal
			modelBuilder.Entity<RightMoveProperty>()
				.Property(x => x.Dates)
					.HasConversion(new ValueConverter<List<DateTime>, string>(
						v => JsonConvert.SerializeObject(v), // Convert to string for persistence
						v => JsonConvert.DeserializeObject<List<DateTime>>(v))); // Convert to List<String> for use

			modelBuilder.Entity<RightMoveProperty>()
				.Property(x => x.Prices)
								.HasConversion(new ValueConverter<List<int>, string>(
									v => JsonConvert.SerializeObject(v), // Convert to string for persistence
									v => JsonConvert.DeserializeObject<List<int>>(v)));
		}
	}
}