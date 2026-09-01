using System.Text.RegularExpressions;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal sealed class Column : NameProperty
    {
        internal static readonly Regex _regex = new(@"^Column\[\@Name\='(?<Name>.+)'\]$", RegexOptions.IgnoreCase);
        internal static readonly Column Any = new(Wildcard_Any);

        internal Column(string name) : base(name) { }

        internal static ValidationResult Validate(string section, bool useRegularIdentifiers) 
            => Validate_Name(section, _regex, useRegularIdentifiers ? Regex_RegularIdentifiers : Regex_DelimitedIdentifiers);

        public override string ToString() => $"Column[@Name='{Name}']";
    }
}
