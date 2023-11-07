using Microsoft.Extensions.DependencyInjection;
using RightMove.Db.Entities;
using RightMove.Db.Services;

namespace RightMove.Db.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterRightMoveDb(this IServiceCollection services)
		{
			services.AddTransient<IDatabaseService<RightMovePropertyEntity>, DatabaseService>();
		}
	}
}
