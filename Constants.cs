using SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes;

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

        internal static readonly string[] ExtendedFiltering_AllowedContexts = [Server_Context, Database_Context, Table_Context, Column_Context];

        // Building ExtendedFilteringProperties - errors
        internal const string ERROR_BUILD_FILTER_UNKNOWN_SECTION = $"Filter contains not allowed section types or has syntax error. Accepted section types: {nameof(Server)}, {nameof(Database)}, {nameof(Table)}, {nameof(Column)}.";
        internal const string ERROR_BUILD_FILTER_DUPLICATED_SECTION = "Filter contains one or more duplicated sections.";
        internal const string ERROR_BUILD_FILTER_INVALID_SECTION_ORDER = $"Sections in filter have invalid order. Correct order based on hierarchy: {nameof(Server)}, {nameof(Database)}, {nameof(Table)}, {nameof(Column)}.";
        internal const string ERROR_BUILD_FILTER_COLUMN_NAME_TOO_LONG = $"The column name in filter exceeds maximum length of {SQL_SERVER_IDENTIFIER_MAX_LENGTH_STR} characters.";
        internal const string ERROR_BUILD_FILTER_TABLE_NAME_TOO_LONG = $"The table name in filter exceeds maximum length of {SQL_SERVER_IDENTIFIER_MAX_LENGTH_STR} characters.";
        internal const string ERROR_BUILD_FILTER_SCHEMA_NAME_TOO_LONG = $"The schema name in filter exceeds maximum length of {SQL_SERVER_IDENTIFIER_MAX_LENGTH_STR} characters.";
        internal const string ERROR_BUILD_FILTER_DATABASE_NAME_TOO_LONG = $"The database name in filter exceeds maximum length of {SQL_SERVER_IDENTIFIER_MAX_LENGTH_STR} characters.";
        internal const string ERROR_BUILD_FILTER_SERVER_NAME_TOO_LONG = $"The server name in filter exceeds maximum length of {SQL_SERVER_IDENTIFIER_MAX_LENGTH_STR} characters.";

        // Validating if ExtendedFilteringProperties can be used for a specific ContextLevel - errors
        internal const string ERROR_FILTER_VALIDATE_CONTEXT_NO_FILTER = "No filter has been provided.";
        internal const string ERROR_FILTER_VALIDATE_CONTEXT_NO_CONTEXT = "No target context has been provided.";
        internal const string ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE = "Target context is not applicable for extended filtering.";
        internal const string ERROR_FILTER_VALIDATE_CONTEXT_FILTER_TARGETS_LOW_CONTEXT = "Filter targets lower context level than the target context.";
    }
}
