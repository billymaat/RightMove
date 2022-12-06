using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RightMove.Db.Entities;
using RightMove.Db.Models;
using RightMove.EF;

namespace RightMove.Db.Repositories
{
	public class RightMovePropertyEFRepository : SqLiteBaseRepository, IRightMovePropertyRepository<RightMoveProperty>
	{
		private readonly RightMoveContext _rightMoveContext;

		public RightMovePropertyEFRepository(IDbConfiguration dbConfiguration, RightMoveContext rightMoveContext) : base(dbConfiguration)
		{
			_rightMoveContext = rightMoveContext;
		}

		public void AddPriceToProperty(int primaryId, int price, string tableName)
		{
			var table =_rightMoveContext.ResultsTable.FirstOrDefault(o => o.Name.Equals(tableName));

			if (table is null)
			{
				// create new table
				table = new Entities.ResultsTable()
				{
					Name = tableName
				};
				_rightMoveContext.ResultsTable.Add(table);
			}

			table.Properties.FirstOrDefault(o => o.Id == primaryId);

		}

		public void CreateTableIfNotExist(string tableName)
		{
			var table = _rightMoveContext.ResultsTable.FirstOrDefault(o => o.Name.Equals(tableName));
			if (table is null)
			{
				// create new table
				table = new Entities.ResultsTable()
				{
					Name = tableName
				};
				_rightMoveContext.ResultsTable.Add(table);
			}

			_rightMoveContext.SaveChanges();
		}

		public List<string> GetAllTableNames()
		{
			return _rightMoveContext.ResultsTable.Select(o => o.Name).ToList();
		}

		public List<RightMoveProperty> LoadProperties(string tableName)
		{
			var rightMovePropertyModels = new List<RightMoveProperty>();
			var table = _rightMoveContext.ResultsTable.FirstOrDefault(o => o.Name.Equals(tableName));

			return table.Properties;
		}

		public void SaveProperty(RightMoveProperty property, string tableName)
		{
			var table = _rightMoveContext.ResultsTable.FirstOrDefault(o => o.Name.Equals(tableName));
			table.Properties.Add(property);
		}


	}
}
