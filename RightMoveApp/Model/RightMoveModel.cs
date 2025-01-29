﻿using System;
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
        private readonly Func<IPropertyPageParser> _propertyParserFactory;
        private readonly IMessenger _messenger;

        public RightMoveModel(RightMoveService rightMoveService,
			Func<IPropertyPageParser> propertyParserFactory,
            IMessenger messenger)
		{
            _rightMoveService = rightMoveService;
            _propertyParserFactory = propertyParserFactory;
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
                _messenger.Send<RightMoveSelectedItemUpdatedMessage>(new RightMoveSelectedItemUpdatedMessage()
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

        public async Task UpdateSelectedRightmoveItem(int rightMoveId, CancellationToken cancellationToken)
		{
			IPropertyPageParser parser = _propertyParserFactory();

			await parser.ParseRightMovePropertyPageAsync(rightMoveId, cancellationToken);
			if (cancellationToken.IsCancellationRequested)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}

			RightMovePropertyFullSelectedItem = parser.RightMoveProperty;
		}

		public async Task<BitmapImage> GetImage(int index, CancellationToken cancellationToken = default(CancellationToken))
		{
			byte[] imageArr = await RightMovePropertyFullSelectedItem.GetImage(index);
			if (imageArr is null)
			{
				return null;
			}

			if (cancellationToken.IsCancellationRequested)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}

			var bitmapImage = ImageHelper.ToImage(imageArr);

			// freeze as accessed from non UI thread
			bitmapImage.Freeze();
			return bitmapImage;
		}
	}
}