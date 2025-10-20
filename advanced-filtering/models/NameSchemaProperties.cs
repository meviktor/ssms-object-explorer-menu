using System;

namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal abstract class NameSchemaProperties : NameProperty
    {
        internal string Schema { get; private set; }

        internal NameSchemaProperties(string name, string schema) : base(name)
        {
            if (string.IsNullOrWhiteSpace(schema))
            {
                throw new ArgumentNullException(nameof(schema), $"Parameter '{nameof(schema)}' cannot be null.");
            }
            Schema = schema;
        }

        protected NameSchemaProperties() : base() => Schema = string.Empty;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;

            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (NameSchemaProperties)obj;
            return 0 == StringComparer.OrdinalIgnoreCase.Compare(Schema, other.Schema) && base.Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = base.GetHashCode();
                hash = hash * 23 + StringComparer.OrdinalIgnoreCase.GetHashCode(Schema ?? string.Empty);
                return hash;
            }
        }
    }
}
