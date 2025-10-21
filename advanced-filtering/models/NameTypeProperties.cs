using System;

namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal abstract class NameTypeProperties : NameProperty
    {
        internal string Type { get; private set; }

        internal NameTypeProperties(string name, string type) : base(name)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type), $"Parameter '{nameof(type)}' cannot be null or whitespace.");
            }
            Type = type;
        }

        protected NameTypeProperties() : base() => Type = string.Empty;

        protected override bool IgnoreRule() => Type == "*" && base.IgnoreRule();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;

            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (NameTypeProperties)obj;
            return 0 == StringComparer.OrdinalIgnoreCase.Compare(Type, other.Type) && base.Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = base.GetHashCode();
                hash = hash * 23 + StringComparer.OrdinalIgnoreCase.GetHashCode(Type ?? string.Empty);
                return hash;
            }
        }
    }
}
