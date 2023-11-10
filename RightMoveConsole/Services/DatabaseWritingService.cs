using System.Collections.Generic;
using RightMove.DataTypes;
using RightMove.Db.Entities;
using RightMove.Db.Services;
using RightMove.Db.Types;

namespace RightMoveConsole.Services
{
	public class DatabaseWritingService : IDatabaseWritingService
	{
		private readonly IDatabaseService<RightMovePropertyEntity> _db;
		public DatabaseWritingService(IDatabaseService<RightMovePropertyEntity> db)
		{
			_db = db;
		}

		public PropertyCounts AddProperty(IList<RightMoveProperty> properties, string tableName)
		{
			var databaseUpdate = _db.AddToDatabase(properties, tableName);
			return databaseUpdate;
		}
	}
}
