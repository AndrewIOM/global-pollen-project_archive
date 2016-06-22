using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace OxPollen.Services.Abstract
{
    public interface IFileStoreService
    {
        SavedImage Upload(IFormFile file);
        List<SavedImage> Upload(IList<IFormFile> files);

        SavedImage UploadBase64Image(string base64File);
        List<SavedImage> UploadBase64Image(List<string> base64Files);
    }

    public class SavedImage
    {
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }

        public SavedImage(string url, string thumbnailUrl)
        {
            Url = url;
            ThumbnailUrl = thumbnailUrl;
        }
    }

}
