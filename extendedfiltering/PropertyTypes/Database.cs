using System.Text.RegularExpressions;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal sealed class Database : NameProperty
    {
        internal static readonly Database Any = new Database("*");
        internal static readonly Regex @Regex = new Regex(@"Database\[\@Name\='(\*|[a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)'\]", RegexOptions.IgnoreCase);

        internal Database(string name) : base(name) { }

        public override string ToString() => $"Database[@Name='{Name}']";
    }
}
