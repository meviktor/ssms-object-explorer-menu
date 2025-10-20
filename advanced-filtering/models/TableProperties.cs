namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class TableProperties : NameSchemaProperties
    {
        internal static TableProperties Empty = new TableProperties();

        protected TableProperties() : base() { }

        internal TableProperties(string name, string schema) : base(name, schema) { }

        public override string ToString() => $"Table[@Name='{Name}' and @Schema='{Schema}']";
    }
}
