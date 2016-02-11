using OxPollen.Services.Abstract;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OxPollen.Options;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OxPollen.Services.Concrete
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

        public List<Tuple<string, string>> Upload(List<string> base64Files)
        {
            var photoUrls = new List<Tuple<string, string>>();
            foreach (var file in base64Files)
            {
                var trimmed = file.Replace(@"data:image/png;base64,", "");
                byte[] bytes = Convert.FromBase64String(trimmed);
                var guid = Guid.NewGuid();
                var filename = guid + "." + "png";
                var filenameThumb = guid + "-thumb." + "png";
                using (var stream = new MemoryStream(bytes))
                {
                    var imageUri = SaveImage(800, stream, filename);
                    var thumbUri = SaveImage(200, stream, filenameThumb);
                    photoUrls.Add(new Tuple<string, string>(imageUri, thumbUri));
                }
            }
            return photoUrls;
        }

        public List<Tuple<string, string>> Upload(IList<IFormFile> files)
        {
            var photoUrls = new List<Tuple<string, string>>();
            foreach (var file in files)
            {
                var uploadedFile = Upload(file);
                if (!string.IsNullOrEmpty(uploadedFile.Item1)) photoUrls.Add(uploadedFile);
            }
            return photoUrls;
        }

        public Tuple<string, string> Upload(IFormFile file)
        {
            var extension = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.TrimStart('"').TrimEnd('"').Split('.').Last();
            if (!IsAcceptedExtension(extension)) return null;
            var guid = Guid.NewGuid();
            var filename = guid + "." + extension;
            var filenameThumb = guid + "-thumb." + extension;
            using (var stream = file.OpenReadStream())
            {
                var imageUri = SaveImage(800, stream, filename);
                var thumbUri = SaveImage(200, stream, filenameThumb);
                return new Tuple<string, string>(filename, filenameThumb);
            }
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

        /// <summary>
        /// Downscales the image and saves to Azure blob storage. Returns Azure URL.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="stream"></param>
        /// <param name="saveFilePath"></param>
        /// <returns></returns>
        private string SaveImage(int size, Stream stream, string saveFilePath)
        {
            double newHeight = 0;
            double newWidth = 0;
            double scale = 0;

            Bitmap curImage = new Bitmap(stream);
            if (curImage.Height > curImage.Width)
            {
                scale = Convert.ToSingle(size) / curImage.Height;
            }
            else
            {
                scale = Convert.ToSingle(size) / curImage.Width;
            }
            if (scale < 0 || scale > 1) { scale = 1; }

            newHeight = Math.Floor(Convert.ToSingle(curImage.Height) * scale);
            newWidth = Math.Floor(Convert.ToSingle(curImage.Width) * scale);

            Bitmap newImage = new Bitmap(curImage, Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));
            Graphics imgDest = Graphics.FromImage(newImage);
            imgDest.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            imgDest.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            imgDest.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            imgDest.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
            EncoderParameters param = new EncoderParameters(1);
            param.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
            imgDest.DrawImage(curImage, 0, 0, newImage.Width, newImage.Height);

            CloudBlockBlob blob = _container.GetBlockBlobReference(saveFilePath);
            MemoryStream memoryStream = new MemoryStream();
            newImage.Save(memoryStream, info[1], param);
            blob.Properties.ContentType = "image/png";
            memoryStream.Position = 0;
            blob.UploadFromStream(memoryStream);

            curImage.Dispose();
            newImage.Dispose();
            imgDest.Dispose();

            return blob.Uri.AbsoluteUri;
        }

    }
}
