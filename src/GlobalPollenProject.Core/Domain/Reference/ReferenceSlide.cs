using System;
using System.Collections.Generic;
using System.Linq;
using GlobalPollenProject.Core.Events;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class ReferenceSlide : IEntity
    {
        private SlideState _state;
        private ICollection<IDomainEvent> _events;

        public ReferenceSlide(
            List<Image> images,
            Taxon identifiedAs,
            double maxDiameter)
        {
            _state = new SlideState();
            _state.TimeAdded = DateTime.UtcNow;
            _state.Images = images.Select(m => m.GetState() as ImageState).ToList();
            _state.TaxonomicIdentity = identifiedAs.GetId();
            _state.MaxDiameter = maxDiameter;
        }

        public ReferenceSlide(SlideState state) 
        { 
            _events = new List<IDomainEvent>();
            _state = state; 
        }

        public ICollection<IDomainEvent> Events { get { return _events; } }
        public object GetState() { return _state; }
    }
}