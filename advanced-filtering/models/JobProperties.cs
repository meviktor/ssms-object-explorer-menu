namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class JobProperties : NameProperty
    {
        internal JobProperties(string name) : base(name) { }

        public override string ToString() => $"Job[@Name='{Name}']";
    }
}
