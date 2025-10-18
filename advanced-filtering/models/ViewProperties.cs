namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class ViewProperties : NameSchemaProperties
    {
        internal ViewProperties(string name, string schema) : base(name, schema) { }

        public override string ToString() => $"View[@Name='{Name}' and @Schema='{Schema}']";
    }
}
