namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal abstract class PropertyBase
    {
        internal bool IsDefined { get; private set; }

        protected PropertyBase(bool isDefined) => IsDefined = isDefined;
    }
}
