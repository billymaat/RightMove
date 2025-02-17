using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RightMove.Desktop.Helpers;
using System.Windows.Media.Imaging;
using RightMove.DataTypes;

namespace RightMove.Desktop.Services
{
    public class RightMoveImageService
    {
        public async Task<BitmapImage> GetImage(RightMoveProperty rightMoveProperty, int index, CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] imageArr = await rightMoveProperty.GetImage(index);
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
