using RightMove.Db.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RightMove.DataTypes;
using RightMove.Db.Types;

namespace RightMove.Db.Services
{
	public class DatabaseService : IDatabaseService<RightMovePropertyEntity>
	{
		private readonly RightMoveContext _context;

		public DatabaseService(RightMoveContext context)
		{
			_context = context;
		}

		public List<RightMovePropertyEntity> LoadProperties(string tableName)
		{
			var table = _context.ResultsTable
				.Include(table => table.Properties)
				.ThenInclude(p => p.Prices)
				.Include(table => table.Properties)
				.FirstOrDefault(o => o.Name.Equals(tableName));
			return table?.Properties;
		}

		public List<string> GetAllTableNames()
		{
			return _context.ResultsTable.Select(o => o.Name).ToList();
		}

		public List<RightMovePropertyEntity> ReducedProperties()
		{
			var reducedProperties = _context.Properties
				.Where(p => p.Prices.Count > 1)
				.Include(p => p.Prices);

			return reducedProperties.ToList();
		}

		private void CreateIfTableDoesNotExist(string tableName)
		{
			var table = _context.ResultsTable.FirstOrDefault(o => o.Name.Equals(tableName));
			if (table is null)
			{
				// create new table
				table = new ResultsTable()
				{
					Name = tableName
				};
				_context.ResultsTable.Add(table);
			}

			_context.SaveChanges();
		}

		public PropertyCounts AddToDatabase(IList<RightMoveProperty> properties, string tableName)
		{
			CreateIfTableDoesNotExist(tableName);

			int newPropertiesCount = 0;
			int updatedPropertiesCount = 0;

			foreach (var property in properties)
			{
				var result = AddToDatabase(property, tableName);
				switch (result)
				{
					case Result.Added:
						newPropertiesCount++;
						break;
					case Result.Updated:
						updatedPropertiesCount++;
						break;
				}
			}

			return new PropertyCounts()
			{
				NewProperties = newPropertiesCount,
				UpdatedProperties = updatedPropertiesCount
			};
		}

		private void AddPriceToProperty(int primaryId, int price, string tableName)
		{
			var table = _context
				.ResultsTable
				.Include(table => table.Properties)
				.ThenInclude(p => p.Prices)
				.FirstOrDefault(o => o.Name.Equals(tableName));

			if (table is null)
			{
				// create new table
				table = new ResultsTable()
				{
					Name = tableName
				};
				_context.ResultsTable.Add(table);
			}

			var property = table.Properties
				.FirstOrDefault(o => o.RightMoveId == primaryId);

			if (property is null)
			{
				// shouldn't get here
				return;
			}

			var prices = new List<DatePrice>(property.Prices);
			prices.Add(new DatePrice()
			{
				Price = price,
				Date = DateTime.Now.ToUniversalTime()
			});

			property.Prices = prices;
			_context.SaveChanges();
		}

		private void SaveProperty(RightMovePropertyEntity property, string tableName)
		{
			var table = _context.ResultsTable.FirstOrDefault(o => o.Name.Equals(tableName));
			table.Properties.Add(property);
			_context.SaveChanges();
		}

		/// <summary>
		/// Add property to database
		/// </summary>
		/// <param name="dbProperties">the properties in the database</param>
		/// <param name="property">the property to add</param>
		/// <returns><see cref="Result.Updated"/> if updated, <see cref="Result.NotModified"/> if not modified,
		/// <see cref="Result.Added"/> if new proprety added</returns>
		public Result AddToDatabase(RightMoveProperty property, string tableName)
		{
			var resultsTableId = _context.ResultsTable
				.FirstOrDefault(o => o.Name.Equals(tableName))?
				.ResultsTableId;

			var matchingProperty = _context.Properties
				.Where(o => o.ResultsTableId == resultsTableId)
				.Where(o => o.RightMoveId == property.RightMoveId)
				.Include(p => p.Prices)
				.FirstOrDefault();

			if (matchingProperty != null)
			{
				// if the price has changed, add the new price
				if (matchingProperty.Prices.Last().Price != property.Price)
				{
					AddPriceToProperty(matchingProperty.RightMoveId, property.Price, tableName);
					return Result.Updated;
				}

				return Result.NotModified;
			}

			// save a new record of the new property
			var rmp = new RightMovePropertyEntity()
			{
				RightMoveId = property.RightMoveId,
				Address = property.Address,
				HouseInfo = property.HouseInfo,
				DateAdded = property.DateAdded.ToUniversalTime(),
				DateReduced = property.DateReduced.ToUniversalTime(),
				Date = DateTime.Now.ToUniversalTime(),
				Prices = new List<DatePrice>()
				{
					new DatePrice()
					{
						Price = property.Price,
						Date = DateTime.Now.ToUniversalTime()
					}
				}
			};



			SaveProperty(rmp, tableName);
			return Result.Added;
		}
	}
}
