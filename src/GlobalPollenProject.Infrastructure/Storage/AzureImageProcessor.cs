using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalPollenProject.Core.Imagery;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Infrastructure.Storage
{
    public class AzureImageProcessor : IImageProcessor
    {        
        // private CloudBlobContainer _container;
        // private readonly ILogger<AzureImageService> _logger;

        // public AzureImageProcessor(IOptions<AzureOptions> optionsAccessor, ILogger<AzureImageService> logger)
        // {
        //     _logger = logger;
        //     Configure(optionsAccessor.Value.AzureConnectionString, optionsAccessor.Value.AzureImageContainer);
        // }

        public Task<List<SavedImage>> Upload(List<Base64Image> image)
        {
            // var trimmed = base64.Replace(@"data:image/png;base64,", "");
            // byte[] bytes = Convert.FromBase64String(trimmed);
            // var guid = Guid.NewGuid();
            // var filename = guid + "." + "png";
            // var filenameThumb = guid + "-thumb." + "png";
            // using (var stream = new MemoryStream(bytes))
            // {
            //     var imageUri = await SaveImage(800, stream, filename);
            //     var thumbUri = await SaveImage(200, stream, filenameThumb);
            //     return new SavedImage(imageUri, thumbUri);
            // }
            throw new NotImplementedException();
        }

        public Task<SavedImage> Upload(Base64Image image)
        {
            // var photoUrls = new List<SavedImage>();
            // foreach (var file in base64Files)
            // {
            //     var saved = await UploadBase64Image(file);
            //     photoUrls.Add(saved);
            // }
            // return photoUrls;
            throw new NotImplementedException();
        }

        private static bool IsAcceptedExtension(string extension)
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