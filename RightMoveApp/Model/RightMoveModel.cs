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
using RightMove.Desktop.Mappers;
using RightMove.Desktop.Services;

namespace RightMove.Desktop.Model
{
	public class RightMoveItemsUpdatedEventArgs : EventArgs
	{
		public List<RightMoveProperty> NewValue { get; set; }
	}

	public class RightMoveSelectedItemUpdatedEventArgs : EventArgs
	{
		public RightMoveProperty SelectedItem { get; set; }
	}

	public class RightMoveModel
	{
		public event EventHandler<RightMoveItemsUpdatedEventArgs> RightMovePropertyItemsUpdated;
		public event EventHandler<RightMoveSelectedItemUpdatedEventArgs> RightMoveSelectedItemUpdated;

        private List<RightMoveProperty> _rightMovePropertyItems;

        public List<RightMoveProperty> RightMovePropertyItems
		{
			get => _rightMovePropertyItems;
			set
			{
				_rightMovePropertyItems = value;
                RightMovePropertyItemsUpdated?.Invoke(this, new RightMoveItemsUpdatedEventArgs()
                {
                    NewValue = value
                });
			}
		}

        private RightMoveProperty _selectedRightMoveProperty;


        public RightMoveProperty SelectedRightMoveProperty
        {
	        get => _selectedRightMoveProperty;
	        set
	        {
		        if (_selectedRightMoveProperty != value)
		        {
			        _selectedRightMoveProperty = value;
			        RightMoveSelectedItemUpdated?.Invoke(this, new RightMoveSelectedItemUpdatedEventArgs()
			        {
				        SelectedItem = value
			        });
		        }
	        }
        }
	}
}