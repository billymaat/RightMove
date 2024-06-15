using RightMove.Db.Entities;
using RightMove.Db.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RightMove.DataTypes;
using RightMove.Db.Types;

namespace RightMove.Db.Services
{
	public class DatabaseService : IDatabaseService<RightMovePropertyEntity>
	{
		private readonly IRightMovePropertyRepository<RightMovePropertyEntity> _db;

		public DatabaseService(IRightMovePropertyRepository<RightMovePropertyEntity> db)
		{
			_db = db;
		}

		public List<RightMovePropertyEntity> LoadProperties(string tableName)
		{
			return _db.LoadProperties(tableName);
		}

		public List<string> GetAllTableNames()
		{
			return _db.GetAllTableNames();
		}

		public List<RightMovePropertyEntity> ReducedProperties()
		{
			var properties = _db.GetAllProperties();
			var reducedProperties = properties.Where(p => p.Prices.Count > 1);
			return reducedProperties.ToList();
		}

		public PropertyCounts AddToDatabase(IList<RightMoveProperty> properties, string tableName)
		{
			_db.CreateTableIfNotExist(tableName);

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

		/// <summary>
		/// Add property to database
		/// </summary>
		/// <param name="dbProperties">the properties in the database</param>
		/// <param name="property">the property to add</param>
		/// <returns><see cref="Result.Updated"/> if updated, <see cref="Result.NotModified"/> if not modified,
		/// <see cref="Result.Added"/> if new proprety added</returns>
		public Result AddToDatabase(RightMoveProperty property, string tableName)
		{
			var matchingProperty = _db.GetPropertyByPropertyId(property.RightMoveId, tableName);

			if (matchingProperty != null)
			{
				// if the price has changed, add the new price
				if (matchingProperty.Prices.Last().Price != property.Price)
				{
					_db.AddPriceToProperty(matchingProperty.RightMoveId, property.Price, tableName);
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



			_db.SaveProperty(rmp, tableName);
			return Result.Added;
		}
	}
}
