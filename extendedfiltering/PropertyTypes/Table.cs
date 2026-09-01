using System.Text.RegularExpressions;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal sealed class Table : NameSchemaProperties
    {
        internal static readonly Regex _regex =  new (@"^Table\[\@Name\='(?<Name>.+)'[ ]+and[ ]+\@Schema\='(?<Schema>.+)'\]$", RegexOptions.IgnoreCase);
        internal static readonly Table Any = new(Wildcard_Any, Wildcard_Any);

        internal Table(string name, string schema) : base(name, schema) { }

        internal static ValidationResult Validate(string section, bool useRegularExpressions) 
            => Validate_NameSchema(section, _regex, useRegularExpressions ? Regex_RegularIdentifiers : Regex_DelimitedIdentifiers);

        public override string ToString() => $"Table[@Name='{Name}' and @Schema='{Schema}']";
    }
}
