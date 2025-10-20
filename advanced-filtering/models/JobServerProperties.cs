namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class JobServerProperties : PropertyBase
    {
        internal static JobServerProperties Empty = new JobServerProperties(false);

        internal JobServerProperties(bool isDefined) : base(isDefined) { }
    }
}
