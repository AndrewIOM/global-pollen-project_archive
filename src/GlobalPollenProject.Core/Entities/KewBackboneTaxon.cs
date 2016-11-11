using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class KewBackboneTaxon : IBackboneTaxon, IEntity
    {
        private KewBackboneTaxon()
        {}

        public int Id { get; private set; }
        public bool IsDeleted { get; set; }
        public List<KewBackboneTaxon> ChildTaxa { get; private set; }
        public KewBackboneTaxon ParentTaxa { get; private set; }
        

        public TaxonomicStatus Status { get; private set; }
        public Rank Rank { get; private set; }
        public string LatinName { get; private set; }
        public string LatinNameAuthorship { get; private set; }

        public List<string> GetNamesForAllTaxonomicRanks()
        {
            var heirarchy = new List<string> { this.LatinName };
            var parent = this.ParentTaxa;
            while (parent != null) 
            {
                heirarchy.Add(parent.LatinName);
                parent.ParentTaxa = parent;
            }
            return heirarchy;
        }

        // /// <summary>
        // /// Gets the latin names for each taxonomic rank including and above the current rank.
        // /// </summary>
        // /// <returns>A dictionary of ranks and their latin names.</returns>
        // public Dictionary<Rank,string> GetHeirarchy()
        // {
        //     throw new NotImplementedException();
        // }

    }
}