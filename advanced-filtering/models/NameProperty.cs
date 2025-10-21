using System;

namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal abstract class NameProperty : PropertyBase
    {
        internal string Name { get; private set; }

        internal NameProperty(string name) : base(true)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"Parameter '{nameof(name)}' cannot be null or whitespace.");
            }
            Name = name;
        }

        protected NameProperty() : base(false) => Name = string.Empty;

        internal bool MatchesWithRule(NameProperty rule) => rule.IgnoreRule() || Equals(rule);

        protected virtual bool IgnoreRule() => Name == "*";

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;

            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (NameProperty)obj;
            return 0 == StringComparer.OrdinalIgnoreCase.Compare(Name, other.Name) && base.Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = base.GetHashCode();
                hash = hash * 23 + StringComparer.OrdinalIgnoreCase.GetHashCode(Name ?? string.Empty);
                return hash;
            }
        }
    }
}
