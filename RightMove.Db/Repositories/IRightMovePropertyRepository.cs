using RightMove.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove.Db.Repositories
{
	public interface IRightMovePropertyRepository
	{
		void SaveProperty(RightMovePropertyModel property);

		List<RightMovePropertyModel> LoadProperties();

		void AddPriceToProperty(int primaryId, int price);
	}
}
