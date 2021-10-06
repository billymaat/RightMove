using System;
using System.Collections.Generic;
using System.Text;
using RightMoveConsole.Models;

namespace RightMoveConsole.Repositories
{
	public interface IRightMovePropertyRepository
	{
		void SaveProperty(RightMovePropertyModel property);

		List<RightMovePropertyModel> LoadProperties();

		void AddPriceToProperty(int primaryId, int price);
	}
}
