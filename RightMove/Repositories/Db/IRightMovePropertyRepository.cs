using RightMove.Models.Db;
using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove.Repositories.Db
{
	public interface IRightMovePropertyRepository
	{
		void SaveProperty(RightMovePropertyModel property);

		List<RightMovePropertyModel> LoadProperties();

		void AddPriceToProperty(int primaryId, int price);
	}
}
