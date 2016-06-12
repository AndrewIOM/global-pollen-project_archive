using OxPollen.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OxPollen.Services.Abstract
{
    public interface ITaxonomyService
    {
        Taxon CreateOrUpdateTaxonomy(string family, string genus, string species);
        IEnumerable<Taxon> GetAll();
        IEnumerable<Taxon> GetAll(Taxonomy rank);
        IEnumerable<Taxon> Suggest(string search);
        Taxon GetById(int id);
        void RemoveTaxon(int id);
        void RefreshConnections(int id);
    }
}
