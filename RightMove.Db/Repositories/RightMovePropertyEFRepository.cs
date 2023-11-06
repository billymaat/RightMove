using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RightMove.Db.Entities;

namespace RightMove.Db.Repositories
{
	public class RightMovePropertyEFRepository : SqLiteBaseRepository, IRightMovePropertyRepository<RightMovePropertyEntity>
	{
		private readonly RightMoveContext _rightMoveContext;

		public RightMovePropertyEFRepository(IDbConfiguration dbConfiguration, RightMoveContext rightMoveContext) : base(dbConfiguration)
		{
			_rightMoveContext = rightMoveContext;
		}

		public void AddPriceToProperty(int primaryId, int price, string tableName)
		{
			var table =_rightMoveContext
				.ResultsTable
				.Include(table => table.Properties)
				.FirstOrDefault(o => o.Name.Equals(tableName));

			if (table is null)
			{
				// create new table
				table = new ResultsTable()
				{
					Name = tableName
				};
				_rightMoveContext.ResultsTable.Add(table);
			}

			var property = table.Properties.FirstOrDefault(o => o.RightMoveId == primaryId);

			if (property is null)
			{
				// shouldn't get here
				return;
			}

			property.Prices.Add(price);
			property.Dates.Add(DateTime.Now);

			// need to notify that the property has changed
			// I think I need to do this because it's a list / because I use a custom conversion?
			_rightMoveContext.Entry(property).Property(p => p.Prices).IsModified = true;
			_rightMoveContext.Entry(property).Property(p => p.Dates).IsModified = true;

			_rightMoveContext.SaveChanges();
		}

		public void CreateTableIfNotExist(string tableName)
		{
			var table = _rightMoveContext.ResultsTable.FirstOrDefault(o => o.Name.Equals(tableName));
			if (table is null)
			{
				// create new table
				table = new ResultsTable()
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

		public List<RightMovePropertyEntity> LoadProperties(string tableName)
		{
			var rightMovePropertyModels = new List<RightMovePropertyEntity>();
			var table = _rightMoveContext.ResultsTable.Include(table => table.Properties).FirstOrDefault(o => o.Name.Equals(tableName));
			return table?.Properties;
		}

		public void SaveProperty(RightMovePropertyEntity property, string tableName)
		{
			var table = _rightMoveContext.ResultsTable.FirstOrDefault(o => o.Name.Equals(tableName));
			table.Properties.Add(property);
			_rightMoveContext.SaveChanges();
		}
	}
}
