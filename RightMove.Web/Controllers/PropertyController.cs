using Microsoft.AspNetCore.Mvc;
using RightMove.Db.Services;

namespace RightMove.Web.Controllers
{
	[ApiController]
	[Route("api/property")]
	public class PropertyController : Controller
	{
		private readonly IDatabaseService _databaseService;
		public PropertyController(IDatabaseService db) : base()
		{
			_databaseService = db;
		}

		[HttpGet("test")]
		public IActionResult TestMethod()
		{
			var properties = _databaseService.LoadProperties("AshtonUnderLyneGreaterManchester");
			return Ok(properties);
		}

		[HttpGet("gettable/{table}")]
		public IActionResult TestTwo(string table)
		{
			var properties = _databaseService.LoadProperties(table);
			return Ok(properties);
		}

		[HttpGet("tables")]
		public IActionResult GetTables()
		{
			var tableNames = _databaseService.GetAllTableNames();
			return Ok(tableNames);
		}
	}
}
