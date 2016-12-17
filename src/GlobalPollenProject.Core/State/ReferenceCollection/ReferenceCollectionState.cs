using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;
using GlobalPollenProject.Core.State.Identity;

namespace GlobalPollenProject.Core.State
{
    public class ReferenceCollectionState : IEntity
    {
        public ReferenceCollectionId Id { get; set; }
        public string Name { get; set; }
        public string OwnedBy { get; set; }
        public string Description { get; set; }
        public string WebAddress { get; set; }
        public string ContactEmail { get; set; }
        public string CountryCode { get; set; }
        public string FocusRegion { get; set; }
        public List<SlideState> Slides { get; set; }
        public OrganisationState Institution { get; set; }
        public UserState Owner { get; set; }
        public bool IsDeleted { get; set; }
    }
}