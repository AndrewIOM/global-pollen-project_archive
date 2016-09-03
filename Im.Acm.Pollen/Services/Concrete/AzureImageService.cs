using Im.Acm.Pollen.Services.Abstract;
using Im.Acm.Pollen.Options;
using ImageProcessorCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Im.Acm.Pollen.Services.Concrete
{
    public class AzureImageService : IFileStoreService
    {
        private CloudBlobContainer _container;
        private readonly ILogger<AzureImageService> _logger;

        public AzureImageService(IOptions<AzureOptions> optionsAccessor, ILogger<AzureImageService> logger)
        {
            _logger = logger;
            Configure(optionsAccessor.Value.AzureConnectionString, optionsAccessor.Value.AzureImageContainer);
        }

        public async Task<SavedImage> UploadBase64Image(string base64)
        {
            var trimmed = base64.Replace(@"data:image/png;base64,", "");
            byte[] bytes = Convert.FromBase64String(trimmed);
            var guid = Guid.NewGuid();
            var filename = guid + "." + "png";
            var filenameThumb = guid + "-thumb." + "png";
            using (var stream = new MemoryStream(bytes))
            {
                var imageUri = await SaveImage(800, stream, filename);
                var thumbUri = await SaveImage(200, stream, filenameThumb);
                return new SavedImage(imageUri, thumbUri);
            }
        }

        public async Task<List<SavedImage>> UploadBase64Image(List<string> base64Files)
        {
            var photoUrls = new List<SavedImage>();
            foreach (var file in base64Files)
            {
                var saved = await UploadBase64Image(file);
                photoUrls.Add(saved);
            }
            return photoUrls;
        }

        public async Task<SavedImage> Upload(IFormFile file)
        {
            var extension = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.TrimStart('"').TrimEnd('"').Split('.').Last();
            if (!IsAcceptedExtension(extension)) return null;
            var guid = Guid.NewGuid();
            var filename = guid + "." + extension;
            var filenameThumb = guid + "-thumb." + extension;
            using (var stream = file.OpenReadStream())
            {
                var imageUri = await SaveImage(800, stream, filename);
                var thumbUri = await SaveImage(200, stream, filenameThumb);
                return new SavedImage(filename, filenameThumb);
            }
        }

        public async Task<List<SavedImage>> Upload(IList<IFormFile> files)
        {
            var photoUrls = new List<SavedImage>();
            foreach (var file in files)
            {
                var uploadedFile = await Upload(file);
                photoUrls.Add(uploadedFile);
            }
            return photoUrls;
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

        private async void Configure(string connectionString, string containerName)
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                _container = blobClient.GetContainerReference(containerName);
                if (await _container.CreateIfNotExistsAsync())
                {
                    await _container.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess =
                                BlobContainerPublicAccessType.Blob
                        });
                    _logger.LogInformation("Successfully created Blob Storage Images Container and made it public");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failure to Create or Configure images container in Blob Storage Service", ex);
            }
        }

        private async Task<string> SaveImage(int size, Stream stream, string saveFilePath)
        {
            double newHeight = 0;
            double newWidth = 0;
            double scale = 0;

            Image image = new Image(stream);
            if (image.Height > image.Width)
            {
                scale = Convert.ToSingle(size) / image.Height;
            }
            else
            {
                scale = Convert.ToSingle(size) / image.Width;
            }
            if (scale < 0 || scale > 1) { scale = 1; }

            newHeight = Math.Floor(Convert.ToSingle(image.Height) * scale);
            newWidth = Math.Floor(Convert.ToSingle(image.Width) * scale);

            MemoryStream memoryStream = new MemoryStream();
            image.Resize(image.Width / 2, image.Height / 2).SaveAsPng(memoryStream);
            memoryStream.Position = 0;

            CloudBlockBlob blob = _container.GetBlockBlobReference(saveFilePath);
            if (await blob.ExistsAsync()) throw new Exception("Blob already exists: " + blob.Uri.AbsoluteUri);
            await blob.UploadFromStreamAsync(memoryStream);
            return blob.Uri.AbsoluteUri;
        }

    }
}
