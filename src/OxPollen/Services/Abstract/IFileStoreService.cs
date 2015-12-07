using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services.Abstract
{
    public interface IFileStoreService
    {
        Task<string> Upload(IFormFile file);
        Task<List<string>> Upload(IList<IFormFile> files);
        List<string> Upload(List<string> base64Files);
    }
}
