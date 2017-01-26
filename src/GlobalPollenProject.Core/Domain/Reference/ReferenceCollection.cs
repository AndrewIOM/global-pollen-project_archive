using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GlobalPollenProject.Core
{
    public class ReferenceCollection
    {
        private ReferenceCollectionState _state;

        public ReferenceCollection(UserId createdBy)
        {
            _state = new ReferenceCollectionState();
            _state.Owner = createdBy;
        }

        public ReferenceCollection(ReferenceCollectionState state)
        {
            _state = state;
        }

        public ICollection<ReferenceSlide> AllSides()
        {
            return new ReadOnlyCollection<ReferenceSlide>(_state.Slides.Select(m => new ReferenceSlide(m)).ToList());
        }

        public ReferenceSlide Slide(SlideId slideId)
        {
            var slideState = _state.Slides.FirstOrDefault(x => x.Id.Equals(slideId.Id));
            return new ReferenceSlide(slideState);
        }

        public void AssignOrganisation(OrganisationId orgId)
        {
            throw new NotImplementedException();
        }

        // public void AssignOrganisation(Organisation org)
        // {
        //     _state.Institution = org;
        //     InstitutionId = org.Id;
        // }

        public void AddEditor(UserId editor)
        {
            throw new NotImplementedException();
        }

        public void ChangeOwner(UserId newOwner)
        {
            _state.Owner = newOwner;
        }

        public void AddSlide(ReferenceSlide slide)
        {
            _state.Slides.Add(slide.GetState() as SlideState);
        }

        public void RemoveSlide(ReferenceSlide slide)
        {
            _state.Slides.Remove(slide.GetState() as SlideState);
        }

    }
}