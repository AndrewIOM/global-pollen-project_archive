using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.AspNet.Hosting;

namespace OxPollen.Services.Concrete
{
    public class ImageService : IFileStoreService
    {
        private readonly IHostingEnvironment _env;
        public ImageService(IHostingEnvironment env)
        {
            _env = env;
        }

        public List<string> Upload(List<string> base64Files)
        {
            var photoUrls = new List<string>();
            foreach (var file in base64Files)
            {
                var trimmed = file.Replace(@"data:image/png;base64,", "");
                byte[] bytes = Convert.FromBase64String(trimmed);
                var guid = Guid.NewGuid();
                var filePath = _env.WebRootPath + "\\user-image-uploads\\" + guid + "." + "png";
                using (var imageFile = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                    photoUrls.Add("\\user-image-uploads\\" + guid + "." + "png");
                }
            }
            return photoUrls;
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
            var filePath = "C:\\OxPollen\\wwwroot\\user-image-uploads\\" + guid + "." + extension;
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
