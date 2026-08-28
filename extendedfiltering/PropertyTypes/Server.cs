using System.Text.RegularExpressions;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal sealed class Server : NameProperty
    {
        internal static readonly Server Any = new(Wildcard_Any);
        internal static readonly Regex @Regex = new($@"^Server\[\@Name\='(\{Wildcard_Any}|[a-zA-Z_\@#][a-zA-Z0-9_#\.\-\@\$]*)'\]$", RegexOptions.IgnoreCase); // Making allowances for mistakes in filter texts regarding wrong casing.

        internal Server(string name) : base(name) { }

        public override string ToString() => $"Server[@Name='{Name}']";
    }
}
