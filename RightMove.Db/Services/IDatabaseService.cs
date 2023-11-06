using System;
using System.Collections.Generic;
using System.Text;
using RightMove.DataTypes;

namespace RightMove.Db.Services
{
	public interface IDatabaseService<T>
	{
		IDbConfiguration DbConfiguration
		{
			get;
		}

		List<T> LoadProperties(string tableName);

		Result AddToDatabase(RightMoveProperty property, string tableName);

		(int, int) AddToDatabase(IList<RightMoveProperty> properties, string tableName);
		List<string> GetAllTableNames();
	}
}
