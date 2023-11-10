using System.Collections.Generic;
using RightMove.DataTypes;
using RightMove.Db.Types;

namespace RightMoveConsole.Services;

public interface IDatabaseWritingService
{
	PropertyCounts AddProperty(IList<RightMoveProperty> properties, string tableName);
}