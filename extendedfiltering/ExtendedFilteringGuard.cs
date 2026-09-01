using SSMSObjectExplorerMenu.objects;

namespace SSMSObjectExplorerMenu.extendedfiltering
{
    internal class ExtendedFilteringGuard
    {
        public static bool IsAllowed(NodeInfo targetNode, string menuItemFilter, bool usingRegularIdentifiers)
        {
            // Extended filtering rules not applicable; there's nothing to check further
            if (!Constants.ExtendedFiltering_AllowedContexts.ContainsKey(targetNode.UrnPath))
                return true;

            var node = ExtendedFilteringProperties.BuildFromNavigationContext(targetNode.NavigationContext, false, out var _);
            var filter = ExtendedFilteringProperties.BuildFromNavigationContext(menuItemFilter, usingRegularIdentifiers, out var _);

            return node != null && filter != null && node.ApplyFiltering(filter);
        }
    }
}
