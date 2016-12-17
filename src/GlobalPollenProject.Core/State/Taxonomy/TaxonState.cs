using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.State.Identity;

namespace GlobalPollenProject.Core.State
{
    public class TaxonState : IEntity
    {
        public TaxonId Id { get; set; }
        public string LatinName { get; private set; }
        public Rank Rank {get; private set; } 
        public int GbifId { get; private set; }
        public int NeotomaId { get; private set; }
        public List<TaxonState> ChildTaxa { get; private set; }
        public TaxonState ParentTaxon { get; private set; }
        public bool IsDeleted { get; set; }

    }
}