using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services.Abstract
{
    public interface IGrainService
    {
        IEnumerable<PollenRecord> GetUnidentifiedGrains();
        PollenRecord GetById(int id);
        void Add(PollenRecord newRecord);
    }
}
