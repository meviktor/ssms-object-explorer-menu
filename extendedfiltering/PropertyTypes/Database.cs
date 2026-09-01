using System.Text.RegularExpressions;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal sealed class Database : NameProperty
    {
        internal static readonly Regex _regex = new(@"^Database\[\@Name\='(?<Name>.+)'\]$", RegexOptions.IgnoreCase);
        internal static readonly Database Any = new(Wildcard_Any);

        internal Database(string name) : base(name) { }

        internal static ValidationResult Validate(string section, bool useRegularIdentifiers)
            => Validate_Name(section, _regex, useRegularIdentifiers ? Regex_RegularIdentifiers : Regex_DelimitedIdentifiers);

        public override string ToString() => $"Database[@Name='{Name}']";
    }
}
