using RightMove.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;
using static RightMoveConsole.Services.DatabaseService;

namespace RightMoveConsole.Services
{
	public interface IDatabaseService
	{
		Result AddToDatabase(RightMoveProperty property);

		(int, int) AddToDatabase(IList<RightMoveProperty> properties);
	}
}
