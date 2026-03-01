using SSMSObjectExplorerMenu.objects;
using System;
using System.Linq;

namespace SSMSObjectExplorerMenu.extendedfiltering
{
    internal class ExtendedFilteringGuard
    {
        public static bool IsAllowed(NodeInfo targetNode, string menuItemFilter)
        {
            // Extended filtering rules not applicable, there's nothing to check further
            if (!Constants.ExtendedFiltering_AllowedContexts.Contains(targetNode.UrnPath))
                return true;

            var targetNodeProps = ExtendedFilteringProperties.BuildFromNavigationContext(targetNode.NavigationContext);
            var menuItemProps = ExtendedFilteringProperties.BuildFromNavigationContext(menuItemFilter);

            return ExtendedFilteringProperties.ComplyTo(targetNodeProps, menuItemProps);
        }
    }
}
