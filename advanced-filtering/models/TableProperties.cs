namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class TableProperties : NameSchemaProperties
    {
        internal TableProperties(string name, string schema) : base(name, schema) { }

        public override string ToString() => $"Table[@Name='{Name}' and @Schema='{Schema}']";
    }
}
