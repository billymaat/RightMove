using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using RightMove.DataTypes;
using RightMove.Desktop.Helpers;
using RightMove.Desktop.Services;
using RightMove.Services;

namespace RightMove.Desktop.Model
{
	public class RightMoveModel : INotifyPropertyChanged
	{
        private readonly RightMoveService _rightMoveService;
        private readonly Func<IPropertyPageParser> _propertyParserFactory;

		public RightMoveModel(RightMoveService rightMoveService,
			Func<IPropertyPageParser> propertyParserFactory)
		{
            _rightMoveService = rightMoveService;
            _propertyParserFactory = propertyParserFactory;
		}

        private RightMoveSearchItemCollection _rightMovePropertyItems;

        public RightMoveSearchItemCollection RightMovePropertyItems
		{
			get => _rightMovePropertyItems;
			set
			{
				_rightMovePropertyItems = value;
				OnPropertyChanged();
			}
		}

		private RightMoveProperty _rightMovePropertyFullSelectedItem;

		public RightMoveProperty RightMovePropertyFullSelectedItem
		{
			get => _rightMovePropertyFullSelectedItem;
			set
			{
				_rightMovePropertyFullSelectedItem = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

        public async Task GetRightMoveItems(SearchParams searchParams)
        {
            var rightMoveItems = await _rightMoveService.GetRightMoveItems(searchParams);
            RightMovePropertyItems = rightMoveItems;
        }

        public async Task GetFullRightMoveItem(int rightMoveId, CancellationToken cancellationToken)
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

		private void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}