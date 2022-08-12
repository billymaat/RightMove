using RightMove.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove.Db.Repositories
{
	public interface IRightMovePropertyRepository
	{
		IDbConfiguration DbConfiguration
		{
			get;
		}

		void CreateTableIfNotExist(string tableName);

		void SaveProperty(RightMovePropertyModel property, string tableName);

		List<RightMovePropertyModel> LoadProperties(string tableName);

		void AddPriceToProperty(int primaryId, int price, string tableName);
	}
}
