using System.Text.RegularExpressions;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal sealed class Table : NameSchemaProperties
    {
        internal static readonly Table Any = new Table(Wildcard_Any, Wildcard_Any);
        internal static readonly Regex @Regex = new Regex($@"Table\[\@Name\='(\{Wildcard_Any}|[a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)' and \@Schema\='(\{Wildcard_Any}|[a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)'\]", RegexOptions.IgnoreCase);

        internal Table(string name, string schema) : base(name, schema) { }

        public override string ToString() => $"Table[@Name='{Name}' and @Schema='{Schema}']";
    }
}
