using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Extensions.Options;
using RightMove.DataTypes;
using RightMove.Factory;
using RightMove.Services;
using RightMoveApp.Helpers;

namespace RightMoveApp.Model
{
	public class RightMoveModel : INotifyPropertyChanged
	{
		private readonly RightMoveParserFactory _parserFactory;
		private readonly Func<IPropertyPageParser> _propertyParserFactory;
		private RightMoveSearchItemCollection _rightMovePropertyItems;
		private readonly AppSettings _appSettings;

		public RightMoveModel(IOptions<AppSettings> appSettings,
            RightMoveParserFactory parserFactory,
			Func<IPropertyPageParser> propertyParserFactory)
		{
			_appSettings = appSettings.Value;
			_parserFactory = parserFactory;
			_propertyParserFactory = propertyParserFactory;
		}

		/// <summary>
		/// Gets a value indicating whether to write to database
		/// </summary>
		public bool WriteToDb => _appSettings.WriteToDb;

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
			var parser = _parserFactory.CreateInstance(searchParams);
			await parser.SearchAsync();
			RightMovePropertyItems = parser.Results;

			if (WriteToDb)
			{
				UpdateDatabase();
			}
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

		private void UpdateDatabase()
		{
			throw new NotImplementedException();
		}

		private void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}