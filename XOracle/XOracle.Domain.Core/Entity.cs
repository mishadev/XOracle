using System;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain.Core
{
    [Serializable]
    public abstract class Entity
    {
        private int? _requestedHashCode;
        private Guid _id;

        public virtual Guid Id
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }

        public bool IsTransient()
        {
            return this.Id == Guid.Empty;
        }

        public void EnsureIdentity()
        {
            if (IsTransient())
                this.Id = IdentityGenerator.NewSequentialGuid();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            Entity item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
                return (int)(_requestedHashCode ?? (_requestedHashCode = this.Id.GetHashCode() ^ 31));
            }
            else
                return base.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
