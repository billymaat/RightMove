using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove.Db.Repositories
{
	public interface IRightMovePropertyRepository<T>
	{
		void CreateTableIfNotExist(string tableName);

		void SaveProperty(T property, string tableName);

		List<T> LoadProperties(string tableName);

		void AddPriceToProperty(int primaryId, int price, string tableName);
		List<string> GetAllTableNames();
	}
}
