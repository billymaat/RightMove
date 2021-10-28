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

		/// <summary>
		/// Save property to db
		/// </summary>
		/// <param name="property">the<see cref="RightMovePropertyModel"/></param>
		public void SaveProperty(RightMovePropertyModel property)
		{
			using (IDbConnection cnn = new SQLiteConnection(GetConnectionString()))
			{
				cnn.Execute("insert into Property (RightMoveId, HouseInfo, Address, Date, Link, Price) values (@RightMoveId, @HouseInfo, @Address, @Date, @Link, @Price)", property);
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
				var property = cnn.QuerySingle<RightMovePropertyModel>("select * from Property where Id = @id", new
				{
					id = primaryId
				});

				if (property is null)
				{
					throw new Exception($"{nameof(property)} should never be null");
				}

				string newPriceString = $"{property.Price}|{price}";
				string newDateString = $"{property.Date}|{DateTime.Now}";
				
				cnn.Execute(@"update Property set Price = @price, Date = @date where Id = @id", new
				{
					price = newPriceString,
					date = newDateString,
					id = primaryId
				});
			}
		}

		private string GetConnectionString()
		{
			return $"Data Source={DbFile};Version=3;";
		}
	}
}
