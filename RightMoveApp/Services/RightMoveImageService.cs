using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RightMove.Desktop.Helpers;
using System.Windows.Media.Imaging;
using RightMove.DataTypes;
using RightMove.Services;

namespace RightMove.Desktop.Services
{
    public class RightMoveImageService
    {
	    private readonly IHttpService _httpService;
	    public RightMoveImageService(IHttpService httpService)
	    {
		    _httpService = httpService;
	    }

        public async Task<BitmapImage> GetImage(RightMoveProperty rightMoveProperty, int index, CancellationToken cancellationToken = default(CancellationToken))
        {
	        if (index >= rightMoveProperty.ImageUrl.Length || index < 0)
	        {
		        return null;
	        }

	        //var imageArr = _httpService.DownloadImage(rightMoveProperty.ImageUrl[index]);
	        var imageArr = await _httpService.DownloadImageAsync(rightMoveProperty.ImageUrl[index], cancellationToken);

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
