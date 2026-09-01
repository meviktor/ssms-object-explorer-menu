using System;
using System.Text.RegularExpressions;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal abstract class NameProperty : IEquatable<NameProperty>
    {
        internal string Name { get; private set; }

        internal NameProperty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"Parameter '{nameof(name)}' cannot be null or whitespace.");
            }
            Name = name;
        }

        protected static ValidationResult Validate_Name(string section, Regex fullSectionRegex, Regex valueRegex)
        {
            var match = fullSectionRegex.Match(section);
            if (!match.Success)
                return new(false, null, []);

            var name = match.Groups["Name"].Value;
            var nameMatch = valueRegex.Match(name);
            if (!nameMatch.Success)
                return new(true, true, []);

            return new(true, false, [("Name", nameMatch.Value)]);
        }

        public bool Equals(NameProperty other)
        {
            if (ReferenceEquals(this, other))
                return true;
            if (other is null)
                return false;
            if (other.GetType() != this.GetType())
                return false;
            return string.Equals(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj) => Equals(obj as NameProperty);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Name);

        public static bool operator ==(NameProperty left, NameProperty right)
            => ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

        public static bool operator !=(NameProperty left, NameProperty right)
            => !(left == right);
    }
}
