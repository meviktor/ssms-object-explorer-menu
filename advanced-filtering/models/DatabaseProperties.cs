namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class DatabaseProperties : NameProperty
    {
        internal static DatabaseProperties Empty = new DatabaseProperties();

        protected DatabaseProperties() : base() { }

        internal DatabaseProperties(string name) : base(name) { }

        public override string ToString() => $"Database[@Name='{Name}']";
    }
}
