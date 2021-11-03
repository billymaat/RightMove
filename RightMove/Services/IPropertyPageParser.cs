using RightMove.DataTypes;
using System.Threading;
using System.Threading.Tasks;

namespace RightMove.Services
{
	public interface IPropertyPageParser
	{
		RightMoveProperty RightMoveProperty
		{
			get;
		}

		Task<bool> ParseRightMovePropertyPageAsync(CancellationToken cancellationToken = default(CancellationToken));

		Task<bool> ParseRightMovePropertyPageAsync(int propertyId, CancellationToken cancellationToken = default(CancellationToken));
	}
}