using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using RightMove.DataTypes;
using RightMove.Db.Models;

namespace RightMove.Db.Repositories
{
	public class RightMovePropertyRepository : SqLiteBaseRepository, IRightMovePropertyRepository
	{
		public RightMovePropertyRepository()
		{

		}

		public void CreateTableIfNotExist()
		{
			using (SQLiteConnection ccn = new SQLiteConnection(GetConnectionString()))
			{
				ccn.Open();

				using (var cmd = new SQLiteCommand(ccn))
				{
					// cmd.CommandText = @"CREATE TABLE cars(id INTEGER PRIMARY KEY, name TEXT, price INT)";
					cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Property(Id INTEGER NOT NULL PRIMARY KEY,
						RightMoveId TEXT NOT NULL,
						HouseInfo TEXT,
						Address TEXT,
						DateAdded TEXT,
						DateReduced TEXT,
						Date TEXT,
						Price TEXT,
						Link TEXT)";
					cmd.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Save property to db
		/// </summary>
		/// <param name="property">the<see cref="RightMovePropertyModel"/></param>
		public void SaveProperty(RightMovePropertyModel property)
		{
			using (IDbConnection cnn = new SQLiteConnection(GetConnectionString()))
			{
				cnn.Execute("insert into Property (RightMoveId, HouseInfo, Address, DateAdded, DateReduced, Date, Price, Link) values (@RightMoveId, @HouseInfo, @Address, @DateAdded, @DateReduced, @Date, @Price, @Link)", property);
			}
		}

		/// <summary>
		/// Load propertues
		/// </summary>
		/// <returns>a list of <see cref="RightMovePropertyModel"/></returns>
		public List<RightMovePropertyModel> LoadProperties()
		{
			using (IDbConnection cnn = new SQLiteConnection(GetConnectionString()))
			{
				var output = cnn.Query<RightMovePropertyModel>("select * from Property", new DynamicParameters());
				return output.ToList();
			}
		}

		/// <summary>
		/// Add price to property
		/// </summary>
		/// <param name="primaryId">the primary id of property in db, to add price</param>
		/// <param name="price">the the price to add</param>
		public void AddPriceToProperty(int primaryId, int price)
		{
			using (IDbConnection cnn = new SQLiteConnection(GetConnectionString()))
			{
				// get the property from the id
				var property = cnn.QuerySingle<RightMovePropertyModel>("select * from Property where Id = @id", new
				{
					id = primaryId
				});

				if (property is null)
				{
					throw new Exception($"{nameof(property)} should never be null");
				}

				// we split the prices in teh database
				// original price|new price
				// original date|new date(now)
				string newPriceString = $"{property.Price}|{price}";
				string newDateString = $"{property.Date}|{DateTime.Now}";

				// write it to the database
				cnn.Execute(@"update Property set Price = @price, Date = @date where Id = @id", new
				{
					price = newPriceString,
					date = newDateString,
					id = primaryId
				});
			}
		}

		/// <summary>
		/// Get the connection string
		/// </summary>
		/// <returns>the connection string</returns>
		private string GetConnectionString()
		{
			return $"Data Source={DbFile};Version=3;";
		}
	}
}
