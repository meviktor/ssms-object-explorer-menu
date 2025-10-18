namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class ColumnProperties : NameProperty
    {
        internal ColumnProperties(string name) : base(name) { }

        public override string ToString() => $"Column[@Name='{Name}']";
    }
}
