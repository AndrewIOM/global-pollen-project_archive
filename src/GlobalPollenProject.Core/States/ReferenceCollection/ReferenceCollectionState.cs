using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class ReferenceCollectionState : IEntity
    {
        private readonly List<SlideState> _slides = new List<SlideState>();
        public ReferenceCollectionId Id { get; set; }
        public List<SlideState> Slides { get { return _slides; } }
        public OrganisationState Institution { get; set; }
        public UserId Owner { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public string OwnedBy { get; set; }
        public string Description { get; set; }
        public string WebAddress { get; set; }
        public string ContactEmail { get; set; }
        public string CountryCode { get; set; }
        public string FocusRegion { get; set; }
    }
}