using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Events;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class User : IEntity
    {
        private UserState _state;

        public User(string title, string firstName, string lastName)
        {
            _state = new UserState();
            _state.Profile.Title = title;
            _state.Profile.FirstName = firstName;
            _state.Profile.LastName = lastName;
            _state.CurrentScore = 0;
        }

        public void ActivatePublicProfile()
        {

        }

        public void DeactivatePublicProfile()
        {

        }

        public UserId GetId()
        {
            return new UserId(_state.Id);
        }

        public void AwardScoreForGrainIdentification(double amount, UnknownGrainId grainId)
        {

        }

        public void RemoveScoreForGrainIdentification(double amount, UnknownGrainId grainId)
        {
            
        }


        public ICollection<IDomainEvent> Events
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object GetState()
        {
            throw new NotImplementedException();
        }
    }
}