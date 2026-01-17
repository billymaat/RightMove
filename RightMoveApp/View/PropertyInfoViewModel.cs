using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RightMove.DataTypes;
using RightMove.Desktop.Helpers;
using RightMove.Desktop.Model;
using RightMove.Desktop.View;
using RightMove.Services;

namespace RightMove.Desktop.ViewModel
{
    public class PropertyInfoViewModel : ObservableRecipient
    {
	    private readonly PropertyPageParserService _propertyPageParserService;
	    private readonly NearbySoldPricesService _nearbySoldPricesService;
	    private readonly RightMoveModel _rightModeModel;
	    public RightMoveImageViewModel RightMoveImageViewModel { get; }

	    public PropertyInfoViewModel(RightMoveImageViewModel rightMoveImageViewModel, 
            PropertyPageParserService propertyPageParserService,
			NearbySoldPricesService nearbySoldPricesService,
			RightMoveModel rightModeModel)
        {
	        _propertyPageParserService = propertyPageParserService;
	        _nearbySoldPricesService = nearbySoldPricesService;
	        _rightModeModel = rightModeModel;
	        RightMoveImageViewModel = rightMoveImageViewModel;
	        _rightModeModel.RightMoveSelectedItemUpdated += (r, m) =>
	        {
                RightMoveSelectedItem = m.SelectedItem;
	        };

	        OpenLink = new RelayCommand<NearbySoldProperty>(ExecuteOpenLink, (NearbySoldProperty nearbySoldProperty) => true);
        }

	    private void ExecuteOpenLink(NearbySoldProperty nearbySoldProperty)
	    {
		    if (nearbySoldProperty == null)
		    {
			    return;
		    }

			BrowserHelper.OpenWebpage(SelectedNearbySoldProperty.Url);
	    }


	    private RightMoveProperty _rightMoveSelectedItem;

        /// <summary>
        /// Gets or sets the selected <see cref="RightMoveViewItem"/>
        /// </summary>
        public RightMoveProperty RightMoveSelectedItem
        {
            get => _rightMoveSelectedItem;
            set
            {
	            if (SetProperty(ref _rightMoveSelectedItem, value))
	            {
		            OnRightMoveItemUpdated();
	            }
            }
        }

        private string _description;

        public string Description
        {
	        get => _description;
	        set => SetProperty(ref _description, value);
        }

        private bool _isLoadingDescription;
        private List<NearbySoldProperty> _nearbySoldProperties;
        private bool _isLoadingNearbySoldPrices;
        private NearbySoldProperty _selectedNearbySoldProperty;

        public bool IsLoadingDescription
        {
	        get => _isLoadingDescription;
	        set => SetProperty(ref _isLoadingDescription, value);
        }

        public bool IsLoadingNearbySoldPrices
        {
	        get => _isLoadingNearbySoldPrices;
	        set => SetProperty(ref _isLoadingNearbySoldPrices, value);
        }

        public List<NearbySoldProperty> NearbySoldProperties
        {
	        get => _nearbySoldProperties;
	        set => SetProperty(ref _nearbySoldProperties, value);
        }

        public NearbySoldProperty SelectedNearbySoldProperty
        {
	        get => _selectedNearbySoldProperty;
	        set => SetProperty(ref _selectedNearbySoldProperty, value);
        }

        public ICommand OpenLink
        {
	        get;
        }

        private async void OnRightMoveItemUpdated()
        {
	        if (RightMoveSelectedItem == null)
	        {
		        return;
	        }

	        IsLoadingDescription = true;
	        var rmp = await _propertyPageParserService.ParseRightMovePropertyPageAsync(RightMoveSelectedItem.RightMoveId);

	        UpdateDescription(rmp);

	        IsLoadingDescription = false;

	        IsLoadingNearbySoldPrices = true;
	        UpdateNearbySoldPrices(rmp);
			IsLoadingNearbySoldPrices = false;
        }

		private void UpdateDescription(RightMoveProperty rmp)
        {
	        Description = null;
	        if (rmp?.Desc == null)
	        {
		        IsLoadingDescription = false;
		        Description = "Failed to get description";
		        return;
	        }

	        Description = rmp.Desc;
        }

		private async void UpdateNearbySoldPrices(RightMoveProperty rmp)
		{

	        var ret = await _nearbySoldPricesService.GetNearbySoldPrices(rmp.NearbySoldPricesUrl);

	        if (ret == null)
	        {
		        NearbySoldProperties = null;
		        return;
	        }

			// want the properties to show most recent first
	        NearbySoldProperties = ret.OrderByDescending(o => o.DateSold).ToList();
		}
    }
}
