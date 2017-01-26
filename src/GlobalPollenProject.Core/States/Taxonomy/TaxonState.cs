using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class TaxonState : IState<Taxon>
    {
        public TaxonId Id { get; set; }
        public string LatinName { get; set; }
        public Rank Rank {get; set; } 
        public int GbifId { get; set; }
        public int NeotomaId { get; set; }
        public List<TaxonState> ChildTaxa { get; set; }
        public TaxonState ParentTaxon { get; set; }
        public bool IsDeleted { get; set; }

        public Taxon ToEntity(IState<Taxon> state)
        {
            return new Taxon(state);
        }
    }
}