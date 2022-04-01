using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SantaTalk
{
    public enum ImageMode
    {
        Camera,
        Gallery
    }

    public class PhotoService
    {
        public async Task<FileResult> GetPhoto(ImageMode mode)
        {
            switch (mode)
            {
                case ImageMode.Camera:
                    if (MediaPicker.IsCaptureSupported)
                        return await MediaPicker.CapturePhotoAsync();
                    break;
                case ImageMode.Gallery:
                    return await MediaPicker.PickPhotoAsync();
            }

            return null;
        }
    }
}