using System;
using System.Collections.Generic;
using System.Text;
using RightMove.Db.Entities;

namespace RightMove.Db.Repositories
{
	public interface IRightMovePropertyRepository<T>
	{
		void CreateTableIfNotExist(string tableName);

		void SaveProperty(T property, string tableName);

		List<T> LoadProperties(string tableName);

		void AddPriceToProperty(int primaryId, int price, string tableName);
		List<string> GetAllTableNames();
		RightMovePropertyEntity GetPropertyByPropertyId(int propertyId, string tableName);
		List<T> GetAllProperties();
	}
}
