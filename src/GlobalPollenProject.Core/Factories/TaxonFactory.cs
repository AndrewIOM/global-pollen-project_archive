using System;
using System.Linq;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.Factories
{
    public class TaxonFactory
    {
        private Func<string, Rank, Taxon, Taxon> _taxonCreate;
        private IRepository<Taxon> _taxonRepo;
        private readonly ITaxonomyBackbone _backbone;
        private readonly IExternalDatabaseLinker _linker;
        public TaxonFactory(Func<string, Rank, Taxon, Taxon> ctorCaller, IRepository<Taxon> taxonRepo, ITaxonomyBackbone backbone, IExternalDatabaseLinker linker)
        {
            _taxonCreate = ctorCaller;
            _taxonRepo = taxonRepo;
            _backbone = backbone;
            _linker = linker;
        }

        // TODO Factory should not return existing records
        // Should fill in parent information though

        public Taxon TryCreateTaxon(string family, string genus, string species)
        {
            Taxon familyTaxon = null;
            Taxon genusTaxon = null;
            Taxon speciesTaxon = null;

//Validate name not null
            if (string.IsNullOrEmpty(family))
            {
                return null;
            }

//Get family taxon
            family = FirstCharToUpper(family);
            familyTaxon = _taxonRepo.FindBy(m => m.LatinName == family && m.Rank == Rank.Family).FirstOrDefault();
            if (familyTaxon == null && _backbone.IsValidTaxon(Rank.Family, family, null, null))
            {
                familyTaxon = _taxonCreate(family, Rank.Family, null);
                familyTaxon.LinkToExternalDatabases(_linker);
                _taxonRepo.Add(familyTaxon);
            }

//Get genus if it is wanted
            if (familyTaxon == null) return null;
            if (!string.IsNullOrEmpty(genus))
            {
                genus = FirstCharToUpper(genus);
                genusTaxon = _taxonRepo.FindBy(m => m.LatinName == genus
                    && m.Rank == Rank.Genus && m.ParentTaxon.LatinName == family).FirstOrDefault();
                if (genusTaxon == null && _backbone.IsValidTaxon(Rank.Genus, family, genus, null))
                {
                    genusTaxon = _taxonCreate(genus, Rank.Genus, familyTaxon);
                    genusTaxon.LinkToExternalDatabases(_linker);
                    _taxonRepo.Add(genusTaxon);
                }
            }
            if (genusTaxon == null) return familyTaxon;

            if (!string.IsNullOrEmpty(species) && !string.IsNullOrEmpty(genus))
            {
                species = FirstCharToLower(species);
                speciesTaxon = _taxonRepo.FindBy(m => m.LatinName == genus + " " + species && m.Rank == Rank.Species
                    && m.ParentTaxon.LatinName == genus).FirstOrDefault();
                if (speciesTaxon == null && _backbone.IsValidTaxon(Rank.Species, family, genus, species))
                {
                    speciesTaxon = _taxonCreate(genus + " " + species, Rank.Species, genusTaxon);
                    speciesTaxon.LinkToExternalDatabases(_linker);
                    _taxonRepo.Add(speciesTaxon);
                }
            }
            if (speciesTaxon == null) return genusTaxon;
            return speciesTaxon;
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

    }
}