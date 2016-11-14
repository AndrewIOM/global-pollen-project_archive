using System.Collections.Generic;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Core
{
    public class Organisation : IEntity
    {
        private Organisation() {}
        public Organisation(string name)
        {
            this.Name = name;
        }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<User> Members { get; set; }
        public string Name { get; set; }
        
    }
}