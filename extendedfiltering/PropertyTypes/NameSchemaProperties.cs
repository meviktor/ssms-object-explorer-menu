using System;
using System.Text.RegularExpressions;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

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

        protected static ValidationResult Validate_NameSchema(string section, Regex fullSectionRegex, Regex valueRegex)
        {
            var match = fullSectionRegex.Match(section);
            if (!match.Success)
                return new(false, null, []);

            var name = match.Groups["Name"].Value;
            var schema = match.Groups["Schema"].Value;

            var nameMatch = valueRegex.Match(name);
            var schemaMatch = valueRegex.Match(schema);
            if (!nameMatch.Success || !schemaMatch.Success)
                return new(true, true, []);

            return new(true, false, [("Name", nameMatch.Value), ("Schema", schemaMatch.Value)]);
        }

        public bool Equals(NameSchemaProperties other) => base.Equals(other)
            && string.Equals(this.Schema, other.Schema, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object other) => Equals(other as NameSchemaProperties);

        public static bool operator ==(NameSchemaProperties left, NameSchemaProperties right)
            => ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

        public static bool operator !=(NameSchemaProperties left, NameSchemaProperties right)
            => !(left == right);

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
