using System.Text.RegularExpressions;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal sealed class Database : NameProperty
    {
        internal static readonly Database Any = new Database(Wildcard_Any);
        internal static readonly Regex @Regex = new Regex($@"Database\[\@Name\='(\{Wildcard_Any}|[a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)'\]", RegexOptions.IgnoreCase);

        internal Database(string name) : base(name) { }

        public override string ToString() => $"Database[@Name='{Name}']";
    }
}
