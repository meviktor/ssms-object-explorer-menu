using System;

namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class NameSchemaProperties : NameProperty
    {
        internal string Schema { get; private set; }

        internal NameSchemaProperties(string name, string schema) : base(name)
        {
            if (string.IsNullOrWhiteSpace(schema))
            {
                throw new ArgumentNullException(nameof(schema), $"Parameter '{nameof(schema)}' cannot be null.");
            }
            Schema = schema;
        }
    }
}
