﻿using RightMove.DataTypes;
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

		List<RightMovePropertyModel> LoadProperties(string tableName);

		Result AddToDatabase(RightMoveProperty property, string tableName);

		(int, int) AddToDatabase(IList<RightMoveProperty> properties, string tableName);
	}
}
