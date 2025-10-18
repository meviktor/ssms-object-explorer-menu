namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class DatabaseProperties : NameProperty
    {
        internal DatabaseProperties(string name) : base(name) { }

        public override string ToString() => $"Database[@Name='{Name}']";
    }
}
