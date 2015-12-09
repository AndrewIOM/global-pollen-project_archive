using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxPollen.Services.Abstract
{
    public interface IGrainService
    {
        void Add(Grain newRecord);
        IEnumerable<Grain> GetUnidentifiedGrains(Taxonomy rank);
        Grain GetById(int id);
        IEnumerable<Grain> GetByUser(string userId);
        Grain MarkDeleted(int id);
        void Update(Grain grain);
    }
}
