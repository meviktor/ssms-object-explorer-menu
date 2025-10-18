using System;

namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal abstract class NameTypeProperties : NameProperty
    {
        internal string Type { get; private set; }

        internal NameTypeProperties(string name, string type) : base(name)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type), $"Parameter '{nameof(type)}' cannot be null or whitespace.");
            }
            Type = type;
        }
    }
}
