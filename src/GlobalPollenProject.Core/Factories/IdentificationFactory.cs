using System;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.Factories
{
    public class IdentificationFactory
    {
        private Func<string, string, string, Rank, User, Identification> _idCreate;
        private readonly ITaxonomyBackbone _backbone;

        public IdentificationFactory(Func<string, string, string, Rank, User, Identification> ctorCaller, ITaxonomyBackbone backbone)
        {
            _idCreate = ctorCaller;
            _backbone = backbone;
        }

        public Identification TryCreateIdentification(string family, string genus, string species, User submittedBy)
        {
            var rank = Rank.Family;
            if (!string.IsNullOrEmpty(family)) return null;
            if (!string.IsNullOrEmpty(genus)) rank = Rank.Genus;
            if (!string.IsNullOrEmpty(genus) && !string.IsNullOrEmpty(species)) rank = Rank.Species;

            if (_backbone.IsValidTaxon(rank, family, genus, species))
            {
                var id = _idCreate(family, genus, species, rank, submittedBy);
                return id;
            }
            return null;
        }

    }
}