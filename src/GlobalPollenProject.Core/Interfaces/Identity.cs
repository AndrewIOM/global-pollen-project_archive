using System;

namespace GlobalPollenProject.Core
{
    public abstract class NumberedIdentity : IEquatable<Identity>, IIdentity<int>
    {
        public NumberedIdentity()
        {
            this.Id = 0;
        }

        public NumberedIdentity(int id)
        {
            this.Id = id;
        }

        public int Id { get; set; }

        public bool Equals(Identity id)
        {
            if (object.ReferenceEquals(this, id)) return true;
            if (object.ReferenceEquals(null, id)) return false;
            return this.Id.Equals(id.Id);
        }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as Identity);
        }

        public override int GetHashCode()
        {
            return (this.GetType().GetHashCode() * 907) + this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return this.GetType().Name + " [Id=" + Id + "]";
        }
    }

    public abstract class Identity : IEquatable<Identity>, IIdentity<string>
    {
        public Identity()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public Identity(string id)
        {
            this.Id = id;
        }

        public string Id { get; set; }

        public bool Equals(Identity id)
        {
            if (object.ReferenceEquals(this, id)) return true;
            if (object.ReferenceEquals(null, id)) return false;
            return this.Id.Equals(id.Id);
        }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as Identity);
        }

        public override int GetHashCode()
        {
            return (this.GetType().GetHashCode() * 907) + this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return this.GetType().Name + " [Id=" + Id + "]";
        }
    }

    public interface IIdentity<T>
    {
        T Id { get; set; }
    }
}