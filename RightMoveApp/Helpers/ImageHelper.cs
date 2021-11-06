using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace RightMoveApp.Helpers
{
	public static class ImageHelper
	{
		public static BitmapImage ToImage(byte[] array)
		{
			using (var ms = new System.IO.MemoryStream(array))
			{
				var image = new BitmapImage();
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad; // here
				image.StreamSource = ms;
				image.EndInit();
				return image;
			}
		}
	}
}
