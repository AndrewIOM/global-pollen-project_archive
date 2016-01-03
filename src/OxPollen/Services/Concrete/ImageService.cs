using OxPollen.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        public List<Tuple<string,string>> Upload(List<string> base64Files)
        {
            var photoUrls = new List<Tuple<string,string>>();
            foreach (var file in base64Files)
            {
                var trimmed = file.Replace(@"data:image/png;base64,", "");
                byte[] bytes = Convert.FromBase64String(trimmed);
                var guid = Guid.NewGuid();
                var filePath = guid + "." + "png";
                var thumbPath = guid + "-thumb." + "png";
                using (var stream = new MemoryStream(bytes))
                {
                    SaveImage(800, stream, _env.WebRootPath + "\\user-image-uploads\\" + filePath);
                    SaveImage(200, stream, _env.WebRootPath + "\\user-image-uploads\\" + thumbPath);
                }
                photoUrls.Add(new Tuple<string, string>(filePath, thumbPath));
            }
            return photoUrls;
        }

        public List<Tuple<string,string>> Upload(IList<IFormFile> files)
        {
            var photoUrls = new List<Tuple<string,string>>();
            foreach (var file in files)
            {
                var uploadedFile = Upload(file);
                if (!string.IsNullOrEmpty(uploadedFile.Item1)) photoUrls.Add(uploadedFile);
            }
            return photoUrls;
        }

        public Tuple<string,string> Upload(IFormFile file)
        {
            var extension = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.TrimStart('"').TrimEnd('"').Split('.').Last();
            if (!IsAcceptedExtension(extension)) return null;
            var guid = Guid.NewGuid();
            var folder = _env.WebRootPath + "\\user -image-uploads\\";
            var filePath = guid + "." + extension;
            var thumbPath = guid + "-thumb." + extension;
            using (var stream = file.OpenReadStream())
            {
                SaveImage(800, stream, folder + filePath);
                SaveImage(200, stream, folder + thumbPath);
            }
            return new Tuple<string,string>(filePath, thumbPath);
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

        private void SaveImage(int size, Stream stream, string saveFilePath)
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
            newImage.Save(saveFilePath, info[1], param);

            curImage.Dispose();
            newImage.Dispose();
            imgDest.Dispose();
        }
    }
}
