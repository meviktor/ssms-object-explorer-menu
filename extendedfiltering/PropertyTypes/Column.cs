using System.Text.RegularExpressions;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal sealed class Column : NameProperty
    {
        internal static readonly Column Any = new Column(Wildcard_Any);
        internal static readonly Regex @Regex = new Regex($@"Column\[\@Name\='(\{Wildcard_Any}|[a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)'\]", RegexOptions.IgnoreCase);

        internal Column(string name) : base(name) { }

        public override string ToString() => $"Column[@Name='{Name}']";
    }
}
