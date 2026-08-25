using SSMSObjectExplorerMenu.objects;
using System;
using System.Linq;

namespace SSMSObjectExplorerMenu.extendedfiltering
{
    internal class ExtendedFilteringGuard
    {
        public static bool IsAllowed(NodeInfo targetNode, string menuItemFilter)
        {
            // Extended filtering rules not applicable; there's nothing to check further
            if (!Constants.ExtendedFiltering_AllowedContexts.Contains(targetNode.UrnPath))
                return true;

            var node = ExtendedFilteringProperties.BuildFromNavigationContext(targetNode.NavigationContext);
            var filter = ExtendedFilteringProperties.BuildFromNavigationContext(menuItemFilter);

            return node.ApplyFiltering(filter);
        }
    }
}
