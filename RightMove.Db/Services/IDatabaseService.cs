using System;
using System.Collections.Generic;
using System.Text;
using RightMove.DataTypes;
using RightMove.Db.Types;

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

		PropertyCounts AddToDatabase(IList<RightMoveProperty> properties, string tableName);
		List<string> GetAllTableNames();
	}
}
