using System;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class IdentificationState : IEntity
    {
        public IdentificationId Id { get; set; }
        public DateTime Time { get; set; }
        public string Family { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public Rank Rank { get; set; }
        public UserId User { get; set; }
        public bool IsDeleted { get; set; }
    }
}