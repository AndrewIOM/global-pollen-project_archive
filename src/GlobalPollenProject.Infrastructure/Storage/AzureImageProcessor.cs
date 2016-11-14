using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GlobalPollenProject.Core.Imagery;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Infrastructure.Options;
using ImageSharp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GlobalPollenProject.Infrastructure.Storage
{
    public class AzureImageProcessor : IImageProcessor
    {        
        private CloudBlobContainer _container;
        private readonly ILogger<AzureImageProcessor> _logger;

        public AzureImageProcessor(IOptions<AzureOptions> optionsAccessor, ILogger<AzureImageProcessor> logger)
        {
            _logger = logger;
            Configure(optionsAccessor.Value.AzureConnectionString, optionsAccessor.Value.AzureImageContainer);
        }

        public async Task<List<SavedImage>> Upload(List<Base64Image> images)
        {
            var photoUrls = new List<SavedImage>();
            foreach (var file in images)
            {
                var saved = await Upload(file);
                photoUrls.Add(saved);
            }
            return photoUrls;
        }

        public async Task<SavedImage> Upload(Base64Image image)
        {
            var trimmed = image.EncodedImage.Replace(@"data:image/png;base64,", "");
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

        private async Task<Uri> SaveImage(int size, Stream stream, string saveFilePath)
        {
            int newHeight = 0;
            int newWidth = 0;
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

            newHeight = (int)Math.Floor(Convert.ToSingle(image.Height) * scale);
            newWidth = (int)Math.Floor(Convert.ToSingle(image.Width) * scale);

            MemoryStream memoryStream = new MemoryStream();
            image.Quality = 256;
            image.Resize(newWidth, newHeight).SaveAsPng(memoryStream);
            memoryStream.Position = 0;

            CloudBlockBlob blob = _container.GetBlockBlobReference(saveFilePath);
            if (await blob.ExistsAsync()) throw new Exception("Blob already exists: " + blob.Uri.AbsoluteUri);
            await blob.UploadFromStreamAsync(memoryStream);
            return blob.Uri;
        }

    }
}