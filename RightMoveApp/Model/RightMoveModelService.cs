using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RightMove.Desktop.Model
{
	public class RightMoveModelService
	{
		private readonly RightMoveModel _rightMoveModel;
		public RightMoveModelService(RightMoveModel rightMoveModel)
		{
			_rightMoveModel = rightMoveModel;
		}

		public void UpdateSelectedRightMoveItem(int rightMoveId, CancellationToken cancellationToken)
		{
			_rightMoveModel.SelectedRightMoveProperty = _rightMoveModel.RightMovePropertyItems.FirstOrDefault(x => x.RightMoveId == rightMoveId);
		}
	}
}
