using System;

namespace SSMSObjectExplorerMenu.advancedfiltering
{
    internal static class AdvancedFilters
    {
        public const string Server = "Server[@Name='{0}']";
        public const string DatabasesFolder = "Server[@Name='{0}']/Folder[@Name='Databases' and @Type='Database']";
        public const string Database = "Server[@Name='{0}']/Database[@Name='{1}']";
        public const string Table = "Server[@Name='{0}']/Database[@Name='{1}']/Table[@Name='{2}' and @Schema='{3}']";
        public const string Column = "Server[@Name='{0}']/Database[@Name='{1}']/Table[@Name='{2}' and @Schema='{3}']/Column[@Name='{4}']";
        public const string UserTablesFolder = "Server[@Name='{0}']/Database[@Name='{1}']/Folder[@Name='UserTables' and @Type='Table']";
        public const string View = "Server[@Name='{0}']/Database[@Name='{1}']/View[@Name='{2}' and @Schema='{3}']";
        public const string StoredProceduresFolder = "Server[@Name='{0}']/Database[@Name='{1}']/Folder[@Name='StoredProcedures' and @Type='StoredProcedure']";
        public const string StoredProcedure = "Server[@Name='{0}']/Database[@Name='{1}']/StoredProcedure[@Name='{2}' and @Schema='{3}']";
        public const string JobServer = "Server[@Name='{0}']/JobServer";
        public const string JobsFolder = "Server[@Name='{0}']/JobServer/Folder[@Name='Jobs' and @Type='Job']";
        public const string Job = "Server[@Name='{0}']/JobServer/Job[@Name='{1}']";

        public static string ServerFilter(string serverName)
            => !string.IsNullOrWhiteSpace(serverName) 
                ? string.Format(Server, serverName) 
                : throw new ArgumentNullException(nameof(serverName), $"Parameter '{nameof(serverName)}' cannot be null.");

        public static string DatabasesFolderFilter(string serverName = "*")
            => string.Format(DatabasesFolder, serverName);

        public static string DatabaseFilter(string databaseName, string serverName = "*")
            => !string.IsNullOrWhiteSpace(databaseName)
                ? string.Format(Database, serverName, databaseName)
                : throw new ArgumentNullException(nameof(databaseName), $"Parameter '{nameof(databaseName)}' cannot be null.");

        public static string TableFilter(string tableName, string schemaName, string databaseName = "*", string serverName = "*")
            => !string.IsNullOrWhiteSpace(tableName)
                ? (!string.IsNullOrWhiteSpace(schemaName) 
                    ? string.Format(Table, serverName, databaseName, tableName, schemaName)
                    : throw new ArgumentNullException(nameof(schemaName), $"Parameter '{nameof(schemaName)}' cannot be null.")
                  )
                : throw new ArgumentNullException(nameof(tableName), $"Parameter '{nameof(tableName)}' cannot be null.");

        public static string ColumnFilter(string columnName, string tableName = "*", string schemaName = "*", string databaseName = "*", string serverName = "*")
            => !string.IsNullOrWhiteSpace(columnName)
                ? string.Format(Column, serverName, databaseName, tableName, schemaName, columnName)
                : throw new ArgumentNullException(nameof(columnName), $"Parameter '{nameof(columnName)}' cannot be null.");

        public static string UserTablesFolderFilter(string databaseName = "*", string serverName = "*")
            => string.Format(UserTablesFolder, serverName, databaseName);

        public static string ViewFilter(string viewName, string schemaName, string databaseName = "*", string serverName = "*")
            => !string.IsNullOrWhiteSpace(viewName)
                ? (!string.IsNullOrWhiteSpace(schemaName)
                    ? string.Format(View, serverName, databaseName, viewName, schemaName)
                    : throw new ArgumentNullException(nameof(schemaName), $"Parameter '{nameof(schemaName)}' cannot be null.")
                  )
                : throw new ArgumentNullException(nameof(viewName), $"Parameter '{nameof(viewName)}' cannot be null.");

        public static string StoredProceduresFolderFilter(string databaseName = "*", string serverName = "*")
            => string.Format(StoredProceduresFolder, serverName, databaseName);

        public static string StoredProcedureFilter(string procedureName, string schemaName, string databaseName = "*", string serverName = "*")
            => !string.IsNullOrWhiteSpace(procedureName)
                ? (!string.IsNullOrWhiteSpace(schemaName)
                    ? string.Format(StoredProcedure, serverName, databaseName, procedureName, schemaName)
                    : throw new ArgumentNullException(nameof(schemaName), $"Parameter '{nameof(schemaName)}' cannot be null.")
                  )
                : throw new ArgumentNullException(nameof(procedureName), $"Parameter '{nameof(procedureName)}' cannot be null.");

        public static string JobServerFilter(string serverName = "*")
            => string.Format(JobServer, serverName);

        public static string JobsFolderFilter(string serverName = "*")
            => string.Format(JobsFolder, serverName);

        public static string JobFilter(string jobName, string serverName = "*")
            => !string.IsNullOrWhiteSpace(jobName)
                ? string.Format(Job, serverName, jobName)
                : throw new ArgumentNullException(nameof(jobName), $"Parameter '{nameof(jobName)}' cannot be null.");
    }
}
