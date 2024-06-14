using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RightMove.Db.Entities;

namespace RightMove.Db.Repositories
{
	public class RightMovePropertyEFRepository : IRightMovePropertyRepository<RightMovePropertyEntity>
	{
		private readonly RightMoveContext _rightMoveContext;

		public RightMovePropertyEFRepository(RightMoveContext rightMoveContext)
		{
			_rightMoveContext = rightMoveContext;
		}

		public void AddPriceToProperty(int primaryId, int price, string tableName)
		{
			var table =_rightMoveContext
				.ResultsTable
				.Include(table => table.Properties)
				.ThenInclude(p => p.Prices)
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

			var property = table.Properties
				.FirstOrDefault(o => o.RightMoveId == primaryId);

			if (property is null)
			{
				// shouldn't get here
				return;
			}

			var prices = new List<DatePrice>(property.Prices);
			prices.Add(new DatePrice()
			{
				Price = price,
				Date = DateTime.Now.ToUniversalTime()
			});

			property.Prices = prices;
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

		public RightMovePropertyEntity GetPropertyByPropertyId(int propertyId, string tableName)
		{
			var resultsTableId = _rightMoveContext.ResultsTable
				.FirstOrDefault(o => o.Name.Equals(tableName))?
				.ResultsTableId;

			var property = _rightMoveContext.Properties
				.Where(o => o.ResultsTableId == resultsTableId)
				.Where(o => o.RightMoveId == propertyId)
				.Include(p => p.Prices)
				.FirstOrDefault();

			return property;
		}

		public List<RightMovePropertyEntity> LoadProperties(string tableName)
		{
			var table = _rightMoveContext.ResultsTable
				.Include(table => table.Properties)
				.FirstOrDefault(o => o.Name.Equals(tableName));
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
