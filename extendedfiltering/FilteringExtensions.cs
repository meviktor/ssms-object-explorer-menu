using SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes;

namespace SSMSObjectExplorerMenu.extendedfiltering
{
    internal static class FilteringExtensions
    {
        internal static bool IsActiveFilter(this Column filter) => filter != null && filter != Column.Any;
        internal static bool IsActiveFilter(this Table filter) => filter != null && filter != Table.Any;
        internal static bool IsActiveFilter(this Database filter) => filter != null && filter != Database.Any;
        internal static bool IsActiveFilter(this Server filter) => filter != null && filter != Server.Any;
    }
}
