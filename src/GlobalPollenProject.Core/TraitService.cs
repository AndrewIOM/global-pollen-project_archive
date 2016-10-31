using System.Linq;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class TraitService : ITraitService
    {
        private IUnitOfWork _uow;
        public TraitService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public double[] ListSizes(int taxonId)
        {
            var taxon = _uow.TaxonRepository.GetById(taxonId);
            if (taxon == null) return null;

            //Current Taxa Sizes
            var sizes = taxon.ReferenceGrains.Select(m => m.MaxSizeNanoMetres).ToList();
            sizes.AddRange(taxon.UserGrains.Select(m => m.MaxSizeNanoMetres));

            //First Child Sizes
            if (taxon.ChildTaxa.Count > 0) {
                sizes.AddRange(taxon.ChildTaxa.SelectMany(m => m.ReferenceGrains.Select(n => n.MaxSizeNanoMetres)));
                sizes.AddRange(taxon.ChildTaxa.SelectMany(m => m.UserGrains.Select(n => n.MaxSizeNanoMetres)));

                var secondChildren = taxon.ChildTaxa.SelectMany(m => m.ChildTaxa);
                //Second child sizes
                foreach (var secondChild in secondChildren) {
                    sizes.AddRange(secondChild.ReferenceGrains.Select(m => m.MaxSizeNanoMetres));
                    sizes.AddRange(secondChild.UserGrains.Select(m => m.MaxSizeNanoMetres));
                }
            }

            return sizes.ToArray();
        }
    }
}
