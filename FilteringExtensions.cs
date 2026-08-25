using SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes;

namespace SSMSObjectExplorerMenu.extendedfiltering
{
    internal static class FilteringExtensions
    {
        extension (Column filter)
        {
            internal bool IsActiveFilter => filter != null && filter != Column.Any;
        }

        extension (Table filter)
        {
            internal bool IsActiveFilter => filter != null && filter != Table.Any;
        }

        extension (Database filter)
        {
            internal bool IsActiveFilter => filter != null && filter != Database.Any;
        }

        extension (Server filter)
        {
            internal bool IsActiveFilter => filter != null && filter != Server.Any;
        }
    }
}
