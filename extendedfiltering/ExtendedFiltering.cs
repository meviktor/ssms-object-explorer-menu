using System.Text.RegularExpressions;

namespace SSMSObjectExplorerMenu.extendedfiltering
{
    internal class ExtendedFiltering
    {
        public const string Wildcard_Any = "*";

        /// <summary>
        /// Naming rule based on the rules of the SQL Server regular (NOT delimited) identifiers.<br/>
        /// As starting the object name with '@', '@@', '#' and '##' have special meaning, starting the identifiers with '@' or '#' is prohibited.
        /// </summary>
        internal static readonly Regex @Regex_RegularIdentifiers = new(@"[\p{L}_][\p{L}\d@$#_]*", RegexOptions.IgnoreCase);

        /// <summary>
        /// Validation regex for delimited identifiers. The only thing checked if quotes (if any) are escaped properly.
        /// </summary>
        internal static readonly Regex @Regex_DelimitedIdentifiers = new("^(?: [^']|'')*", RegexOptions.IgnoreCase);

        private const string Server = "Server[@Name='{0}']";
        private const string Database = "Server[@Name='{0}']/Database[@Name='{1}']";
        private const string Table = "Server[@Name='{0}']/Database[@Name='{1}']/Table[@Name='{2}' and @Schema='{3}']";
        private const string Column = "Server[@Name='{0}']/Database[@Name='{1}']/Table[@Name='{2}' and @Schema='{3}']/Column[@Name='{4}']";

        internal static string ServerFilter(string serverName) => string.Format(Server, serverName);

        internal static string DatabaseFilter(string databaseName, string serverName = "*") => string.Format(Database, serverName, databaseName);

        internal static string TableFilter(string tableName, string schemaName, string databaseName = "*", string serverName = "*")
            => string.Format(Table, serverName, databaseName, tableName, schemaName);

        internal static string ColumnFilter(string columnName, string tableName = "*", string schemaName = "*", string databaseName = "*", string serverName = "*")
            => string.Format(Column, serverName, databaseName, tableName, schemaName, columnName);
    }
}
