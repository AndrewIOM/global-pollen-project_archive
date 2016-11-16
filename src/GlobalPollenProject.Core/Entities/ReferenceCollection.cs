using System.Collections.Generic;
using System.Linq;
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
        public string Institution { get; set; }
        public string OwnedBy { get; set; }
        public string Description { get; set; }
        public string WebAddress { get; set; }
        public string ContactEmail { get; set; }

        // More complex
        public string CountryCode { get; set; }
        public string FocusRegion { get; set; }
        public List<ReferenceSlide> Slides { get { return _slides; } }
        public User Owner { get; private set; }

        public bool IsDeleted { get; set; }

        internal void AddSlide(ReferenceSlide slide)
        {
            _slides.Add(slide);
        }

        internal void RemoveSlide(ReferenceSlide slide)
        {
            _slides.Remove(slide);
        }

        internal List<ReferenceSlide> ListSlides()
        {
            return _slides.ToList();
        }

    }
}