using Microsoft.AspNetCore.Mvc;
using RightMove.Db.Entities;
using RightMove.Db.Services;
using RightMove.Web.Dto;

namespace RightMove.Web.Controllers
{
	[ApiController]
	[Route("api/property")]
	public class PropertyController : Controller
	{
		private readonly IDatabaseService<RightMovePropertyEntity> _databaseService;
		public PropertyController(IDatabaseService<RightMovePropertyEntity> db) : base()
		{
			_databaseService = db;
		}

		[HttpGet("test")]
		public IActionResult TestMethod()
		{
			var properties = _databaseService.LoadProperties("AshtonUnderLyneGreaterManchester");

			var dto = properties.Select(p => p.ToDto());
			return Ok(dto);
		}

		[HttpGet("gettable/{table}")]
		public IActionResult TestTwo(string table)
		{
			var properties = _databaseService.LoadProperties(table);
			var dto = properties.Select(p => p.ToDto());
			return Ok(dto);
		}

		[HttpGet("tables")]
		public IActionResult GetTables()
		{
			var tableNames = _databaseService.GetAllTableNames();
			return Ok(tableNames);
		}
	}
}
