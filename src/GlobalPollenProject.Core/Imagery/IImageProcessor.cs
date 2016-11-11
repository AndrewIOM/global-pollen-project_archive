
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalPollenProject.Core.Imagery;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface IImageProcessor
    {
        Task<SavedImage> Upload(Base64Image image);
        Task<List<SavedImage>> Upload(List<Base64Image> image);
    }

    public class SavedImage
    {
        public Uri FullSizeImage { get; private set; }
        public Uri ThumbnailImage { get; private set; }

        public SavedImage(Uri url, Uri thumbnailUrl)
        {
            FullSizeImage = url;
            ThumbnailImage = thumbnailUrl;
        }
    }

}
