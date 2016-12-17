using System;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core.State
{
    public class IdentificationState : IEntity
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Family { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public Rank Rank { get; set; }
        public User User { get; set; }
        public bool IsDeleted { get; set; }
    }
}