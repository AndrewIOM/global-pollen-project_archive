using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services.Abstract
{
    public interface IFileStoreService
    {
        Tuple<string, string> Upload(IFormFile file);
        List<Tuple<string,string>> Upload(IList<IFormFile> files);
        List<Tuple<string,string>> Upload(List<string> base64Files);
    }
}
