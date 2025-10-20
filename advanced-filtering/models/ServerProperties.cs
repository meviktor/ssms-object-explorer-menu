namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class ServerProperties : NameProperty
    {
        internal static ServerProperties Empty = new ServerProperties();

        protected ServerProperties() : base() { }

        internal ServerProperties(string name) : base(name) { }

        public override string ToString() => $"Server[@Name='{Name}']";
    }
}
