using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.Dnx.Runtime;

namespace OxPollen.Services.Concrete
{
    public class ImageService : IFileStoreService
    {
        private readonly IApplicationEnvironment _env;
        public ImageService(IApplicationEnvironment env)
        {
            _env = env;
        }

        public async Task<List<string>> Upload(IList<IFormFile> files)
        {
            var photoUrls = new List<string>();
            foreach (var file in files)
            {
                var uploadedFile = await Upload(file);
                if (!string.IsNullOrEmpty(uploadedFile)) photoUrls.Add(uploadedFile);
            }
            return photoUrls;
        }

        public async Task<string> Upload(IFormFile file)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
            if (!IsAcceptedExtension(fileName)) return null;
            var guid = Guid.NewGuid();
            var filePath = _env.ApplicationBasePath + "\\..\\..\\..\\wwwroot\\user-image-uploads\\" + guid + ".jpg";
            await file.SaveAsAsync(filePath);
            return "\\user-image-uploads\\" + guid + ".jpg";
        }

        private bool IsAcceptedExtension(string fileName)
        {
            if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") || fileName.EndsWith(".gif")
                || fileName.EndsWith(".png")) return true;
            return false;
        }
    }
}
