using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.PlatformAbstractions;

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
            var extension = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.TrimStart('"').TrimEnd('"').Split('.').Last();
            if (!IsAcceptedExtension(extension)) return null;
            var guid = Guid.NewGuid();
            var filePath = "C:\\Projects\\OxPollen\\src\\OxPollen\\wwwroot\\user-image-uploads\\" + guid + "." + extension;
            await file.SaveAsAsync(filePath);
            return "\\user-image-uploads\\" + guid + "." + extension;
        }

        private bool IsAcceptedExtension(string extension)
        {
            var acceptedExtensions = new List<string>() { "jpg", "jpeg", "gif", "png" };
            foreach (var acceptedExtension in acceptedExtensions)
            {
                if (string.Equals(extension, acceptedExtension, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }
    }
}
