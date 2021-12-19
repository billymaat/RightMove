using RightMove.DataTypes;
using RightMove.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static RightMove.Db.Services.DatabaseService;

namespace RightMove.Db.Services
{
	public interface IDatabaseService
	{
		IDbConfiguration DbConfiguration
		{
			get;
		}

		bool CreateTableIfNotExists();

		List<RightMovePropertyModel> LoadProperties();

		Result AddToDatabase(RightMoveProperty property);

		(int, int) AddToDatabase(IList<RightMoveProperty> properties);
	}
}
