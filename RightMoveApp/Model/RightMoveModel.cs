using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Messaging;
using RightMove.DataTypes;
using RightMove.Desktop.Helpers;
using RightMove.Desktop.Messages;
using RightMove.Desktop.Services;
using RightMove.Services;

namespace RightMove.Desktop.Model
{
	public class RightMoveModel
	{
        private readonly RightMoveService _rightMoveService;
        private readonly IMessenger _messenger;

        public RightMoveModel(RightMoveService rightMoveService,
            IMessenger messenger)
		{
            _rightMoveService = rightMoveService;
            _messenger = messenger;
        }

        private List<RightMoveProperty> _rightMovePropertyItems;

        public List<RightMoveProperty> RightMovePropertyItems
		{
			get => _rightMovePropertyItems;
			set
			{
				_rightMovePropertyItems = value;
                _messenger.Send(new RightMovePropertyItemsUpdatedMessage()
                {
                    NewValue = value
                });
            }
		}

		private RightMoveProperty _rightMovePropertyFullSelectedItem;

		public RightMoveProperty RightMovePropertyFullSelectedItem
		{
			get => _rightMovePropertyFullSelectedItem;
			set
			{
				_rightMovePropertyFullSelectedItem = value;
                _messenger.Send<RightMoveFullSelectedItemUpdatedMessage>(new RightMoveFullSelectedItemUpdatedMessage()
                {
                    NewValue = value
                });
            }
		}

        public async Task UpdateRightMoveItems(SearchParams searchParams)
        {
            var rightMoveItems = await _rightMoveService.GetRightMoveItems(searchParams);
            var items = rightMoveItems.ToList();
            RightMovePropertyItems = items;
        }

        public async Task UpdateSelectedRightMoveItem(int rightMoveId, CancellationToken cancellationToken)
        {
            var fullProperty = await _rightMoveService.GetFullRightMoveItem(rightMoveId, cancellationToken);
            RightMovePropertyFullSelectedItem = fullProperty;
		}
	}
}