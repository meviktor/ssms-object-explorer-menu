using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSObjectExplorerMenu.extendedfiltering
{
    internal class ExtendedFiltering
    {
        private const string Server = "Server[@Name='{0}']";
        private const string Database = "Server[@Name='{0}']/Database[@Name='{1}']";
        private const string Table = "Server[@Name='{0}']/Database[@Name='{1}']/Table[@Name='{2}' and @Schema='{3}']";
        private const string Column = "Server[@Name='{0}']/Database[@Name='{1}']/Table[@Name='{2}' and @Schema='{3}']/Column[@Name='{4}']";

        internal static string ServerFilter(string serverName)
           => !string.IsNullOrWhiteSpace(serverName)
               ? string.Format(Server, serverName)
               : throw new ArgumentNullException(nameof(serverName), $"Parameter '{nameof(serverName)}' cannot be null.");

        internal static string DatabaseFilter(string databaseName, string serverName = "*")
          => !string.IsNullOrWhiteSpace(databaseName)
              ? string.Format(Database, serverName, databaseName)
              : throw new ArgumentNullException(nameof(databaseName), $"Parameter '{nameof(databaseName)}' cannot be null.");

        internal static string TableFilter(string tableName, string schemaName, string databaseName = "*", string serverName = "*")
            => !string.IsNullOrWhiteSpace(tableName)
                ? (!string.IsNullOrWhiteSpace(schemaName)
                    ? string.Format(Table, serverName, databaseName, tableName, schemaName)
                    : throw new ArgumentNullException(nameof(schemaName), $"Parameter '{nameof(schemaName)}' cannot be null.")
                  )
                : throw new ArgumentNullException(nameof(tableName), $"Parameter '{nameof(tableName)}' cannot be null.");

        internal static string ColumnFilter(string columnName, string tableName = "*", string schemaName = "*", string databaseName = "*", string serverName = "*")
            => !string.IsNullOrWhiteSpace(columnName)
                ? string.Format(Column, serverName, databaseName, tableName, schemaName, columnName)
                : throw new ArgumentNullException(nameof(columnName), $"Parameter '{nameof(columnName)}' cannot be null.");
    }
}
