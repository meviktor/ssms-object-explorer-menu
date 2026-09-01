namespace SSMSObjectExplorerMenu.extendedfiltering
{
    internal class ValidationResult(bool IsIdentified, bool? HasSyntaxErrors, (string Name, string Value)[] ExtractedProperties)
    {
        internal bool IsIdentified { get; } = IsIdentified;
        internal bool? HasSyntaxErrors { get; } = HasSyntaxErrors;
        internal (string Name, string Value)[] ExtractedProperties { get; } = ExtractedProperties;
    }
}
