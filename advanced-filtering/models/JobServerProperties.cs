namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class JobServerProperties : PropertyBase
    {
        internal static JobServerProperties Empty = new JobServerProperties(false);

        internal bool MatchesWithRule(JobServerProperties rule) => Equals(rule);

        internal JobServerProperties(bool isDefined) : base(isDefined) { }
    }
}
