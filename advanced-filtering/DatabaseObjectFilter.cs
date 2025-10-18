using SSMSObjectExplorerMenu.advancedfiltering.models;
using SSMSObjectExplorerMenu.enums;

namespace SSMSObjectExplorerMenu.advancedfiltering
{
    internal class DatabaseObjectFilter
    {
        internal MenuItemContext Context { get; private set; }

        internal ServerProperties Server { get; private set; } = null;

        internal FolderProperties Folder { get; private set; } = null;

        internal DatabaseProperties Database { get; private set; } = null;

        internal TableProperties Table { get; private set; } = null;

        internal ColumnProperties Column { get; private set; } = null;

        internal ViewProperties View { get; private set; } = null;

        internal StoredProcedureProperties StoredProcedure { get; private set; } = null;

        internal JobProperties Job { get; private set; } = null;

        internal DatabaseObjectFilter(
            MenuItemContext context,
            ServerProperties server = null,
            FolderProperties folder = null,
            DatabaseProperties database = null,
            TableProperties table = null,
            ColumnProperties column = null,
            ViewProperties view = null,
            StoredProcedureProperties storedProcedure = null,
            JobProperties job = null)
        {
            Context = context;
            Server = server;
            Folder = folder;
            Database = database;
            Table = table;
            Column = column;
            View = view;
            StoredProcedure = storedProcedure;
            Job = job;
        }

        // TODO: add a validate method which:
        // - gets a string representation if the item's NavigationContext
        // - builds some model from it
        // - checks if the model complies with the filter
    }
}
