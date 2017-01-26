using System;
using System.Collections.Generic;
using GlobalPollenProject.Core.Events;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class ScoreRecord : IEntity
    {
        public ScoreRecord(string origin, double total)
        {
            this.ScoreTotal = total;
            this.ScoreOrigin = origin;
            this.When = DateTime.Now;
        }

        public string ScoreOrigin { get; set; }
        public double ScoreTotal { get; set; }
        public DateTime When { get; set; }


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