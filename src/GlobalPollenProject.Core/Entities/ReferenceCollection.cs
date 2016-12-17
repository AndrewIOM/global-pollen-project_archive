using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class ReferenceCollection : IEntity
    {
        private ReferenceCollection() {}
        public ReferenceCollection(User createdBy)
        {
            this.Owner = createdBy;
        }

        private readonly List<ReferenceSlide> _slides = new List<ReferenceSlide>();

        public int Id { get; set; }

        // Simple
        public string Name { get; set; }
        public string OwnedBy { get; set; }
        public string Description { get; set; }
        public string WebAddress { get; set; }
        public string ContactEmail { get; set; }

        // More complex
        public string CountryCode { get; set; }
        public string FocusRegion { get; set; }
        public List<ReferenceSlide> Slides { get { return _slides; } }

        // Associated Organisation
        public int InstitutionId { get; private set; }
        public Organisation Institution { get; private set; }

        public void AssignOrganisation(Organisation org)
        {
            Institution = org;
            InstitutionId = org.Id;
        }

        // GPP 'Owner' of the collection
        public User Owner { get; private set; }

        public void ChangeOwner(User newOwner)
        {
            Owner = newOwner;
        }

        public bool IsDeleted { get; set; }

        public void AddSlide(ReferenceSlide slide)
        {
            _slides.Add(slide);
        }

        public void RemoveSlide(ReferenceSlide slide)
        {
            _slides.Remove(slide);
        }

    }
}