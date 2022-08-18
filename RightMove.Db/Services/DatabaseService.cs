using RightMove.DataTypes;
using RightMove.Db.Models;
using RightMove.Db.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RightMove.Db.Services
{
	public class DatabaseService : IDatabaseService
	{
		private readonly IRightMovePropertyRepository _db;

		public DatabaseService(IRightMovePropertyRepository db)
		{
			_db = db;
		}

		public enum Result
		{
			Added,
			Updated,
			NotModified,
			Failed
		}

		public IDbConfiguration DbConfiguration => _db.DbConfiguration;

		public List<RightMovePropertyModel> LoadProperties(string tableName)
		{
			return _db.LoadProperties(tableName);
		}

		public List<String> GetAllTableNames()
		{
			return _db.GetAllTableNames();
		}

		public (int, int) AddToDatabase(IList<RightMoveProperty> properties, string tableName)
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

		public Result AddToDatabase(RightMoveProperty property, string tableName)
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
		private Result AddToDatabase(List<RightMovePropertyModel> dbProperties, RightMoveProperty property, string tableName)
		{
			var matchingProperty = dbProperties.FirstOrDefault(o => o.RightMoveId.Equals(property.RightMoveId));

			if (matchingProperty != null)
			{
				// if the price has changed, add the new price
				if (matchingProperty.Prices.Last() != property.Price)
				{
					_db.AddPriceToProperty(matchingProperty.Id, property.Price, tableName);
					return Result.Updated;
				}

				return Result.NotModified;
			}

			// save a new record of the new property
			_db.SaveProperty(new RightMovePropertyModel(property), tableName);
			return Result.Added;
		}
	}
}
