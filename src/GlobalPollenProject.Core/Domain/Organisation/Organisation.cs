using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Events;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class Organisation : IEntity
    {
        private OrganisationState _state;
        private ICollection<IDomainEvent> _events;

        public Organisation(string name, User createdBy)
        {
            _state = new OrganisationState();
            _events = new List<IDomainEvent>();
            _state.Name = name;
            _state.IsActive = true;
            AddMember(createdBy);
        }

        public ICollection<IDomainEvent> Events { get { return _events; } }

        public void AddMember(User user)
        {
            throw new NotImplementedException();
        }

        public void RemoveMember(User user)
        {
            throw new NotImplementedException();
        }

        public void UpdateGroupInfo(string name, string description, string url)
        {
            throw new NotImplementedException();
        }

        public object GetState()
        {
            throw new NotImplementedException();
        }
    }
}