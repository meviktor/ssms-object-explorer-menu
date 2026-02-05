using System;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal abstract class NameSchemaProperties : NameProperty, IEquatable<NameSchemaProperties>
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

        public bool Equals(NameSchemaProperties other) => base.Equals(other)
            && string.Equals(this.Schema, other.Schema, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object other) => Equals(other as NameSchemaProperties);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + base.GetHashCode();
                hash = hash * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(Schema);
                return hash;
            }
        }
    }
}
