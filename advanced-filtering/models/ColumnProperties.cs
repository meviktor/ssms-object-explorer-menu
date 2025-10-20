namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class ColumnProperties : NameProperty
    {
        internal static ColumnProperties Empty = new ColumnProperties();

        protected ColumnProperties() : base() { }

        internal ColumnProperties(string name) : base(name) { }

        public override string ToString() => $"Column[@Name='{Name}']";
    }
}
