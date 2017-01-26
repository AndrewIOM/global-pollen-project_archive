using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Events;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class ResearcherProfile : IEntity
    {
        public ResearcherProfile(string firstName, string lastName, string title)
        {
            this.Title = title;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public object GetState()
        {
            throw new NotImplementedException();
        }

        public bool Enabled { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<IDomainEvent> Events { get { throw new NotImplementedException(); }}
    }
}