using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.Utilities;
using GlobalPollenProject.Core.Models;

namespace GlobalPollenProject.Core
{
    public class TaxonomyService : ITaxonomyService
    {
        private IUnitOfWork _uow;
        private ITaxonomyBackbone _backbone;
        public TaxonomyService(IUnitOfWork uow, ITaxonomyBackbone backbone)
        {
            _uow = uow;
            _backbone = backbone;
        }

        public Taxon CreateOrUpdateTaxonomy(string family, string genus, string species)
        {
            Taxon familyTaxon = null;
            Taxon genusTaxon = null;
            Taxon speciesTaxon = null;

            if (string.IsNullOrEmpty(family))
            {
                //Log error
                return null;
            }

            family = FirstCharToUpper(family);
            familyTaxon = _uow.TaxonRepository.Find(m => m.LatinName == family && m.Rank == Taxonomy.Family).FirstOrDefault();
            if (familyTaxon == null && _backbone.IsValidTaxon(Taxonomy.Family, family, null, null))
            {
                var gbifID = GbifUtility.GetGbifId(Taxonomy.Family, family, null, null);
                var neotomaId = NeotomaUtility.GetTaxonId(family);
                familyTaxon = new Taxon()
                {
                    LatinName = family,
                    Rank = Taxonomy.Family,
                    GbifId = gbifID.Result,
                    NeotomaId = neotomaId.Result
                };
                _uow.TaxonRepository.Add(familyTaxon);
                _uow.SaveChanges();
            }

            if (familyTaxon == null) return null;
            if (!string.IsNullOrEmpty(genus))
            {
                genus = FirstCharToUpper(genus);
                genusTaxon = _uow.TaxonRepository.Find(m => m.LatinName == genus
                    && m.Rank == Taxonomy.Genus && m.ParentTaxa.LatinName == family).FirstOrDefault();
                if (genusTaxon == null && _backbone.IsValidTaxon(Taxonomy.Genus, family, genus, null))
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Genus,
                        familyTaxon != null ? familyTaxon.LatinName : null, genus, null);
                    var neotomaId = NeotomaUtility.GetTaxonId(genus);
                    genusTaxon = new Taxon()
                    {
                        LatinName = genus,
                        Rank = Taxonomy.Genus,
                        ParentTaxa = familyTaxon,
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result,
                    };
                    _uow.TaxonRepository.Add(genusTaxon);
                    _uow.SaveChanges();
                }
            }
            if (genusTaxon == null) return familyTaxon;

            if (!string.IsNullOrEmpty(species) && !string.IsNullOrEmpty(genus))
            {
                species = FirstCharToLower(species);
                speciesTaxon = _uow.TaxonRepository.Find(m => m.LatinName == genus + " " + species && m.Rank == Taxonomy.Species
                    && m.ParentTaxa.LatinName == genus).FirstOrDefault();
                if (speciesTaxon == null && _backbone.IsValidTaxon(Taxonomy.Species, family, genus, species))
                {
                    var gbifID = GbifUtility.GetGbifId(Taxonomy.Species,
                        familyTaxon != null ? familyTaxon.LatinName : null, genus, species);
                    var neotomaId = NeotomaUtility.GetTaxonId(genus + " " + species);
                    speciesTaxon = new Taxon()
                    {
                        LatinName = genus + " " + species,
                        Rank = Taxonomy.Species,
                        ParentTaxa = genusTaxon,
                        GbifId = gbifID.Result,
                        NeotomaId = neotomaId.Result
                    };
                    _uow.TaxonRepository.Add(speciesTaxon);
                    _uow.SaveChanges();
                }
            }
            if (speciesTaxon == null) return genusTaxon;
            return speciesTaxon;
        }

        public IEnumerable<Taxon> GetAll()
        {
            var result = _uow.TaxonRepository.GetAll();
            return result;
        }

        public IEnumerable<Taxon> GetAll(Taxonomy rank)
        {
            var result = _uow.TaxonRepository.Find(m => m.Rank == rank);
            return result;
        }

        public Taxon GetById(int id)
        {
            return _uow.TaxonRepository.GetById(id);
        }

        public void RemoveTaxon(int id)
        {
            var taxon = _uow.TaxonRepository.GetById(id);
            if (taxon == null) return;
            _uow.TaxonRepository.Delete(taxon);
            _uow.SaveChanges();
        }

        public IEnumerable<Taxon> Suggest(string search)
        {
            var result = _uow.TaxonRepository.Find(m => m.LatinName.Contains(search));
            return result;
        }

        private string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.First().ToString().ToUpper() + input.Substring(1).ToLower();
        }

        private string FirstCharToLower(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.First().ToString().ToLower() + input.Substring(1).ToLower();
        }

        public void RefreshConnections(int id)
        {
            var taxon = _uow.TaxonRepository.GetById(id);
            if (taxon == null) return;

            string family = "";
            string genus = "";
            string species = "";
            if (taxon.Rank == Taxonomy.Family)
            {
                family = taxon.LatinName;
            } else if (taxon.Rank == Taxonomy.Genus)
            {
                genus = taxon.LatinName;
                family = taxon.ParentTaxa.LatinName;
            } else
            {
                species = taxon.LatinName;
                genus = taxon.ParentTaxa.LatinName;
                family = taxon.ParentTaxa.ParentTaxa.LatinName;
            }

            var gbifId = GbifUtility.GetGbifId(taxon.Rank, family, genus, species).Result;
            var neotomaId = NeotomaUtility.GetTaxonId(taxon.LatinName).Result;

            taxon.GbifId = gbifId;
            taxon.NeotomaId = neotomaId;
            _uow.TaxonRepository.Update(taxon);
            _uow.SaveChanges();
        }
    }
}
