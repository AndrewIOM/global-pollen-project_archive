using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.State.Identity;

namespace GlobalPollenProject.Core.State
{
    public class BackboneTaxonState : IBackboneTaxon, IEntity
    {
        public BackboneId Id { get; private set; }
        public bool IsDeleted { get; set; }
        public List<BackboneTaxonState> ChildTaxa { get; set; }
        public BackboneTaxonState ParentTaxa { get; set; }
        public TaxonomicStatus Status { get; set; }
        public Rank Rank { get; set; }
        public string LatinName { get; set; }
        public string LatinNameAuthorship { get; set; }
        public string Reference { get; set; }
        public string ReferenceUrl { get; set; }

    }
}