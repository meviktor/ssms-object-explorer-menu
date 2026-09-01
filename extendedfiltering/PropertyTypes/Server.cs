using System.Text.RegularExpressions;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

namespace SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes
{
    internal sealed class Server : NameProperty
    {
        internal static readonly Regex _regex = new(@"^Server\[\@Name\='(?<Name>.+)'\]$", RegexOptions.IgnoreCase); // Making allowances for mistakes in filter texts regarding wrong casing.
        internal static readonly Server Any = new(Wildcard_Any);
        
        internal Server(string name) : base(name) { }

        /// <summary>
        /// This function only validates on the node format and if the name has unescaped quote(s), nothing else.</br>
        /// As server name may appear in a lot of different formats (FQDN, <machine_name>\<SQL_Server_instance_name>, IP address, etc.) there is no further validation.
        /// </summary>
        internal static ValidationResult Validate(string section) => Validate_Name(section, _regex, Regex_DelimitedIdentifiers);

        public override string ToString() => $"Server[@Name='{Name}']";
    }
}
