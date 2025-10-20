namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class JobProperties : NameProperty
    {
        internal static JobProperties Empty = new JobProperties();

        protected JobProperties() : base() { }

        internal JobProperties(string name) : base(name) { }

        public override string ToString() => $"Job[@Name='{Name}']";
    }
}
