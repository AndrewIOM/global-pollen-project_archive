using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class ReferenceCollection : IEntity
    {
        public ReferenceCollection(User createdBy)
        {
            this.Owner = createdBy;
        }

        private readonly IList<ReferenceSlide> _slides = new List<ReferenceSlide>();

        public int Id { get; set; }

        // Simple
        public string Name { get; set; }
        public string Institution { get; set; }
        public string OwnedBy { get; set; }
        public string Description { get; set; }
        public string WebAddress { get; set; }
        public string ContactEmail { get; set; }

        // More complex
        public string CountryCode { get; private set; }
        public string FocusRegion { get; private set; }
        public List<ReferenceSlide> Slides { get; private set; }
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