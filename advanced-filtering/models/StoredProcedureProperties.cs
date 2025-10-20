namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class StoredProcedureProperties : NameSchemaProperties
    {
        internal static StoredProcedureProperties Empty = new StoredProcedureProperties();

        protected StoredProcedureProperties() : base() { }

        internal StoredProcedureProperties(string name, string schema) : base(name, schema) { }

        public override string ToString() => $"StoredProcedure[@Name='{Name}' and @Schema='{Schema}']";
    }
}
