namespace SSMSObjectExplorerMenu
{
    internal class Constants
    {
        internal const string DateTime2_FormatString = "yyyy-MM-dd HH:mm:ss.fffffff";
        internal const string DateTimeOffset_FormatString = "yyyy-MM-dd HH:mm:ss.fffffff zzz";

        internal const string Server_Context = "Server";
        internal const string Database_Context = "Server/Database";
        internal const string Table_Context = "Server/Database/Table";
        internal const string Column_Context = "Server/Database/Table/Column";

        internal static readonly string[] ExtendedFiltering_AllowedContexts = new string[] { Server_Context, Database_Context, Table_Context, Column_Context };
    }
}
