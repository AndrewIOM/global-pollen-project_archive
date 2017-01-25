using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalPollenProject.WebUI.Services.Abstract
{
    public interface IFileStoreService
    {
        Task<SavedImage> Upload(IFormFile file);
        Task<List<SavedImage>> Upload(IList<IFormFile> files);

        Task<SavedImage> UploadBase64Image(string base64File);
        Task<List<SavedImage>> UploadBase64Image(List<string> base64Files);
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
