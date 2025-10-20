using System;

namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal abstract class NameProperty : PropertyBase
    {
        internal string Name { get; private set; }

        internal NameProperty(string name) : base(true)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"Parameter '{nameof(name)}' cannot be null or whitespace.");
            }
            Name = name;
        }

        protected NameProperty() : base(false) => Name = string.Empty;
    }
}
