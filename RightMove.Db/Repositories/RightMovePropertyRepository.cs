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
		public RightMovePropertyRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
		{
		}

		public void CreateTableIfNotExist(string tableName)
		{
			using (SQLiteConnection ccn = new SQLiteConnection(GetConnectionString()))
			{
				ccn.Open();

				using (var cmd = new SQLiteCommand(ccn))
				{
					// cmd.CommandText = @"CREATE TABLE cars(id INTEGER PRIMARY KEY, name TEXT, price INT)";
					cmd.CommandText = @"CREATE TABLE IF NOT EXISTS " + tableName + @"(Id INTEGER NOT NULL PRIMARY KEY,
						RightMoveId TEXT NOT NULL,
						HouseInfo TEXT,
						Address TEXT,
						DateAdded TEXT,
						DateReduced TEXT,
						Date TEXT,
						Price TEXT)";
					cmd.ExecuteNonQuery();
				}
			}
		}

		/// <summary>
		/// Save property to db
		/// </summary>
		/// <param name="property">the<see cref="RightMovePropertyModel"/></param>
		public void SaveProperty(RightMovePropertyModel property, string tableName)
		{
			using (IDbConnection cnn = new SQLiteConnection(GetConnectionString()))
			{
				cnn.Execute("insert into " + tableName + " (RightMoveId, HouseInfo, Address, DateAdded, DateReduced, Date, Price) values (@RightMoveId, @HouseInfo, @Address, @DateAdded, @DateReduced, @Date, @Price)", property);
			}
		}

		public List<string> GetAllTableNames()
		{
			using (SQLiteConnection cnn = new SQLiteConnection(GetConnectionString()))
			{
				cnn.Open();
				var schema = cnn.GetSchema("Tables");
				var tableNames = new List<string>();

				foreach (DataRow row in schema.Rows)
				{
					tableNames.Add(row[2].ToString());
				}

				return tableNames;
			}
		}

		/// <summary>
		/// Load propertues
		/// </summary>
		/// <returns>a list of <see cref="RightMovePropertyModel"/></returns>
		public List<RightMovePropertyModel> LoadProperties(string tableName)
		{
			using (IDbConnection cnn = new SQLiteConnection(GetConnectionString()))
			{
				var output = cnn.Query<RightMovePropertyModel>("select * from " + tableName, new DynamicParameters());
				return output.ToList();
			}
		}

		/// <summary>
		/// Add price to property
		/// </summary>
		/// <param name="primaryId">the primary id of property in db, to add price</param>
		/// <param name="price">the the price to add</param>
		public void AddPriceToProperty(int primaryId, int price, string tableName)
		{
			using (IDbConnection cnn = new SQLiteConnection(GetConnectionString()))
			{
				// get the property from the id
				var property = cnn.QuerySingle<RightMovePropertyModel>("select * from " + tableName + " where Id = @id", new
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
				string newDateString = $"{property.Date}|{DateTime.Now.ToString("dd/MM/yyyy")}";

				// write it to the database
				cnn.Execute(@"update " + tableName + " set Price = @price, Date = @date where Id = @id", new
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
