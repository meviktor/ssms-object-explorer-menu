using System.Text.RegularExpressions;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal sealed class Table : NameSchemaProperties
    {
        internal static readonly Table Any = new(Wildcard_Any, Wildcard_Any);
        internal static readonly Regex @Regex = new($@"^Table\[\@Name\='(\{Wildcard_Any}|[a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)' and \@Schema\='(\{Wildcard_Any}|[a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)'\]$", RegexOptions.IgnoreCase);

        internal Table(string name, string schema) : base(name, schema) { }

        public override string ToString() => $"Table[@Name='{Name}' and @Schema='{Schema}']";
    }
}
