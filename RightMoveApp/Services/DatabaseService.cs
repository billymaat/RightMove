using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Extensions.Options;
using RightMove;
using RightMove.DataTypes;

namespace RightMoveApp.Services
{
	/// <summary>
	/// Interface for database services
	/// </summary>
	public interface IDatabaseService
	{
		/// <summary>
		/// Load properties from data base
		/// </summary>
		/// <returns>the properties stored in the database</returns>
		List<RightMoveProperty> LoadProperties();

		/// <summary>
		/// Save properties in the database
		/// </summary>
		/// <param name="properties">the properties to save</param>
		/// <param name="uniqueOnly">true to store unique (by id) only</param>
		/// <param name="checkPrice">try to store unique (by both id and by price) only</param>
		void SaveProperties(List<RightMoveProperty> properties, bool uniqueOnly = true, bool checkPrice = true);

		/// <summary>
		/// Save properties in the database
		/// </summary>
		/// <param name="property">the property to save</param>
		/// <param name="uniqueOnly">true to store unique (by id) only</param>
		/// <param name="checkPrice">try to store unique (by both id and by price) only</param>
		void SaveProperty(RightMoveProperty property, bool uniqueOnly = true, bool checkPrice = true);
	}

	public class DatabaseService : IDatabaseService
	{
		/// <summary>
		/// The application settings, that contains the db info
		/// </summary>
		private readonly AppSettings _appSettings;
		
		public DatabaseService(IOptions<AppSettings> options)
		{
			_appSettings = options.Value;
		}
		
		public List<RightMoveProperty> LoadProperties()
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				var output = cnn.Query<RightMoveProperty>("select * from Property", new DynamicParameters());
				return output.ToList();
			}
		}

		public void SaveProperties(List<RightMoveProperty> properties, bool uniqueOnly = true, bool checkPrice = true)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				foreach (var property in properties)
				{
					AddPropertyWithConnection(cnn, property, uniqueOnly, checkPrice);
				}
			}
		}
		public void SaveProperty(RightMoveProperty property, bool uniqueOnly = true, bool checkPrice = true)
		{
			using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
			{
				AddPropertyWithConnection(cnn, property, uniqueOnly, checkPrice);
			}
		}
		
		private string LoadConnectionString(string id = "Default")
		{
			return _appSettings.ConnectionString;
		}
		
		private void AddPropertyWithConnection(IDbConnection cnn, RightMoveProperty property, bool checkUnique = true, bool checkPrice = true)
		{
			bool addToDb = true;

			if (checkUnique || checkPrice)
			{
				if (!checkPrice)
				{
					addToDb = cnn.ExecuteScalar<bool>("select count(1) from Property where RightMoveId=@id", new {id = property.RightMoveId});
				}

				else
				{
					var matchingProperties = cnn.Query<RightMoveProperty>("select * from Property where RightMoveId=@id", new { id = property.RightMoveId }).ToList();
					if (matchingProperties.Count > 0)
					{
						// going to have to find the latest added property and check the price
						var latestProperty = matchingProperties.First();
						if (latestProperty.Price == property.Price)
						{
							addToDb = false;
						}
					}
				}
			}

			if (!addToDb)
			{
				cnn.Execute("insert into Property (RightMoveId, Link, HouseInfo, Desc, Agent, Date, Price) " +
				            "values (@RightMoveId, @Link, @HouseInfo, @Desc, @Agent, @Date, @Price)", property);
			}
		}
	}
}
