namespace SSMSObjectExplorerMenu.advancedfiltering.models
{
    internal class FolderProperties : NameTypeProperties
    {
        internal FolderProperties(string name, string type) : base(name, type) { }

        public override string ToString() => $"Folder[@Name='{Name}' and @Type='{Type}']";
    }
}
