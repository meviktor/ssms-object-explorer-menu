using System.Text.RegularExpressions;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal sealed class Table : NameSchemaProperties
    {
        internal static readonly Table Any = new Table("*", "*");
        internal static readonly Regex @Regex = new Regex(@"Table\[\@Name\='(\*|[a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)' and \@Schema\='(\*|[a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)'\]", RegexOptions.IgnoreCase);

        internal Table(string name, string schema) : base(name, schema) { }

        public override string ToString() => $"Table[@Name='{Name}' and @Schema='{Schema}']";
    }
}
