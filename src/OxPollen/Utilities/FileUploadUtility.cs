﻿using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Utilities
{
    public static class FileUploadUtility
    {
        public static bool IsImage(IFormFile file)
        {
            var extension = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.TrimStart('"').TrimEnd('"').Split('.').Last();
            return IsAcceptedExtension(extension) ? true : false;
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
