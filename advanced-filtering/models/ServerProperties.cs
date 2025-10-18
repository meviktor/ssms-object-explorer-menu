namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class ServerProperties : NameProperty
    {
        internal ServerProperties(string name) : base(name) { }

        public override string ToString() => $"Server[@Name='{Name}']";
    }
}
