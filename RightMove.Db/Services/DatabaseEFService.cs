using RightMove.Db.Entities;
using RightMove.Db.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RightMove.Db.Services
{
	public class DatabaseEFService : IDatabaseService<RightMoveProperty>
	{
		private readonly IRightMovePropertyRepository<RightMoveProperty> _db;

		public DatabaseEFService(IRightMovePropertyRepository<RightMoveProperty> db)
		{
			_db = db;
		}

		public IDbConfiguration DbConfiguration => _db.DbConfiguration;

		public List<RightMoveProperty> LoadProperties(string tableName)
		{
			return _db.LoadProperties(tableName);
		}

		public List<String> GetAllTableNames()
		{
			return _db.GetAllTableNames();
		}

		public (int, int) AddToDatabase(IList<DataTypes.RightMoveProperty> properties, string tableName)
		{
			Console.WriteLine(tableName);
			_db.CreateTableIfNotExist(tableName);

			var dbProperties = _db.LoadProperties(tableName);
			int newPropertiesCount = 0;
			int updatedPropertiesCount = 0;

			foreach (var property in properties)
			{
				var result = AddToDatabase(dbProperties, property, tableName);
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

			return (newPropertiesCount, updatedPropertiesCount);
		}

		public Result AddToDatabase(DataTypes.RightMoveProperty property, string tableName)
		{
			var dbProperties = _db.LoadProperties(tableName);
			return AddToDatabase(dbProperties, property, tableName);
		}

		/// <summary>
		/// Add property to database
		/// </summary>
		/// <param name="dbProperties">the properties in the database</param>
		/// <param name="property">the property to add</param>
		/// <returns><see cref="Result.Updated"/> if updated, <see cref="Result.NotModified"/> if not modified,
		/// <see cref="Result.Added"/> if new proprety added</returns>
		private Result AddToDatabase(List<RightMoveProperty> dbProperties, DataTypes.RightMoveProperty property, string tableName)
		{
			var matchingProperty = dbProperties?.FirstOrDefault(o => o.RightMoveId.Equals(property.RightMoveId));

			if (matchingProperty != null)
			{
				// if the price has changed, add the new price
				if (matchingProperty.Prices.Last() != property.Price)
				{
					_db.AddPriceToProperty(matchingProperty.RightMoveId, property.Price, tableName);
					return Result.Updated;
				}

				return Result.NotModified;
			}

			// save a new record of the new property
			var rmp = new RightMoveProperty()
			{
				RightMoveId = property.RightMoveId,
				Address = property.Address,
				HouseInfo = property.HouseInfo,
				DateAdded = property.DateAdded,
				DateReduced = property.DateReduced,
				Date = DateTime.Now,
				Prices = new List<int>() { property.Price },
				Dates = new List<DateTime>() { DateTime.Now }
			};



			_db.SaveProperty(rmp, tableName);
			return Result.Added;
		}
	}
}
