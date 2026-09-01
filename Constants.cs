using SSMSObjectExplorerMenu.extendedfiltering;
using SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes;
using System.Collections.Generic;

namespace SSMSObjectExplorerMenu
{
    internal class Constants
    {
        internal const byte SQL_SERVER_IDENTIFIER_MAX_LENGTH = 128;
        private const string SQL_SERVER_IDENTIFIER_MAX_LENGTH_STR = "128";

        internal const string DateTime2_FormatString = "yyyy-MM-dd HH:mm:ss.fffffff";
        internal const string DateTimeOffset_FormatString = "yyyy-MM-dd HH:mm:ss.fffffff zzz";

        internal const string Server_Context = "Server";
        internal const string Database_Context = "Server/Database";
        internal const string Table_Context = "Server/Database/Table";
        internal const string Column_Context = "Server/Database/Table/Column";

        // TODO: maybe move it to utils
        internal static readonly Dictionary<string, ContextLevel> ExtendedFiltering_AllowedContexts = new()
        {
            { Server_Context, ContextLevel.Server },
            { Database_Context, ContextLevel.Database },
            { Table_Context, ContextLevel.Table },
            { Column_Context, ContextLevel.Column }
        };

        // Building ExtendedFilteringProperties - errors
        internal const string ERROR_BUILD_FILTER_UNKNOWN_SECTION = $"Filter contains not known section type(s) or has syntax error. Accepted section types: {nameof(Server)}, {nameof(Database)}, {nameof(Table)}, {nameof(Column)}.";
        internal const string ERROR_BUILD_FILTER_DUPLICATED_SECTION = "Filter contains one or more duplicated sections.";
        internal const string ERROR_BUILD_FILTER_INVALID_SECTION_ORDER = $"Sections have invalid order. Correct order based on hierarchy: {nameof(Server)}, {nameof(Database)}, {nameof(Table)}, {nameof(Column)}.";
        internal const string ERROR_BUILD_FILTER_COLUMN_NAME_TOO_LONG = $"The column name exceeds maximum length of {SQL_SERVER_IDENTIFIER_MAX_LENGTH_STR} characters.";
        internal const string ERROR_BUILD_FILTER_TABLE_NAME_TOO_LONG = $"The table name exceeds maximum length of {SQL_SERVER_IDENTIFIER_MAX_LENGTH_STR} characters.";
        internal const string ERROR_BUILD_FILTER_SCHEMA_NAME_TOO_LONG = $"The schema name exceeds maximum length of {SQL_SERVER_IDENTIFIER_MAX_LENGTH_STR} characters.";
        internal const string ERROR_BUILD_FILTER_DATABASE_NAME_TOO_LONG = $"The database name exceeds maximum length of {SQL_SERVER_IDENTIFIER_MAX_LENGTH_STR} characters.";
        internal const string ERROR_BUILD_FILTER_SERVER_NAME_TOO_LONG = $"The server name exceeds maximum length of {SQL_SERVER_IDENTIFIER_MAX_LENGTH_STR} characters.";

        internal const string ERROR_BUILD_FILTER_COLUMN_NAME_UNESCAPED_QUOTES = $"The column name contains unescaped quotes (').";
        internal const string ERROR_BUILD_FILTER_TABLE_NAME_UNESCAPED_QUOTES = $"The table name  contains unescaped quotes (').";
        internal const string ERROR_BUILD_FILTER_SCHEMA_NAME_UNESCAPED_QUOTES = $"The schema name contains unescaped quotes (').";
        internal const string ERROR_BUILD_FILTER_DATABASE_NAME_UNESCAPED_QUOTES = $"The database name contains unescaped quotes (').";
        internal const string ERROR_BUILD_FILTER_SERVER_NAME_UNESCAPED_QUOTES = $"The server name contains unescaped quotes (').";

        internal const string ERROR_BUILD_FILTER_COLUMN_NAME_NOT_A_REGULAR_IDENTIFIER = $"The column name is not an SQL Server regular identifier.";
        internal const string ERROR_BUILD_FILTER_TABLE_NAME_NOT_A_REGULAR_IDENTIFIER = $"The table name is not an SQL Server regular identifier.";
        internal const string ERROR_BUILD_FILTER_SCHEMA_NAME_NOT_A_REGULAR_IDENTIFIER = $"The schema name is not an SQL Server regular identifier.";
        internal const string ERROR_BUILD_FILTER_DATABASE_NAME_NOT_A_REGULAR_IDENTIFIER = $"The database name is not an SQL Server regular identifier.";

        // Validating if ExtendedFilteringProperties can be used for a specific ContextLevel - errors
        internal const string ERROR_FILTER_VALIDATE_CONTEXT_NO_FILTER = "No filter has been provided.";
        internal const string ERROR_FILTER_VALIDATE_CONTEXT_NO_CONTEXT = "No target context has been provided.";
        internal const string ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE = "Target context is not applicable for extended filtering.";
        internal const string ERROR_FILTER_VALIDATE_CONTEXT_FILTER_TARGETS_LOW_CONTEXT = "Filter has condition(s) for lower level(s) than the target menu item (e.g. it has condition for columns while the menu item applies for tables).";
        internal const string ERROR_FILTER_VALIDATE_LOW_CONTEXT_SEGMENT = "Filter has segment(s) for lower level(s) than the target menu item (e.g. it has a Column[...] section while the menu item applies for tables).";
    }
}
