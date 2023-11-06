using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMove.Db.Entities
{
	public class ResultsTable
	{
		public int ResultsTableId { get; set; }

		public string? Name { get; set; }

		public List<RightMovePropertyEntity> Properties { get; set; } = new List<RightMovePropertyEntity>();
	}
}
