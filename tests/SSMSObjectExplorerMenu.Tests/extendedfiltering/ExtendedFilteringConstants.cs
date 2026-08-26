using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

namespace SSMSObjectExplorerMenu.Tests.extendedfiltering
{
    internal static class TestSamples
    {
        // Valid idenifiers for all section properties (Name, Schema)
        internal const string Identifier_Valid = "Id";
        internal const string Identifier_Valid_Digits = "Id2";
        internal const string Identifier_Valid_WithSpecialChars = "@Identifier_Name#";
        internal const string Identifier_Valid_Any = $"{Wildcard_Any}";

        // Invalid idenifiers for all section properties (Name, Schema)
        internal const string Identifier_Invalid_Empty = "";
        internal const string Identifier_Invalid_WhiteSpaceOnly = " ";
        internal const string Identifier_Invalid_StartsWithDigit = "1Id";
        internal const string Identifier_Invalid_ContainsSpace = "my column";
        internal const string Identifier_Invalid_ContainsComma = "price,vat";
        internal const string Identifier_Invalid_ContainsParentheses = "(dentif)er";
        internal const string Identifier_Invalid_ContainsBracket = "[dentif]er";
        internal const string Identifier_Invalid_ContainsNewLine = "Id\nentifier";
        internal const string Identifier_Invalid_ContainsTab = "Id\tentifier";
        internal const string Identifier_Invalid_LongerThan128Chars = "@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789";

        // Identifiers valid for server sections, invalid for others
        internal const string Identifier_ContainsHyphen = "a-b";
        internal const string Identifier_ContainsDot = "a.b";

        // Invalid section samples
        internal const string Section_Invalid_Type_NotKnown = "NotKnownSection[@Name='Unknown']";
        internal const string Section_Invalid_Property_NotKnown = "Column[@Unkown='Value']";
        internal const string Section_Invalid_BadSyntax_MissingAt = "Column[Name='Value']";
        internal const string Section_Invalid_BadSyntax_MissingQuotes = "Column[@Name=Value]";
        internal const string Section_Invalid_BadSyntax_MissingEqualSign = "Column[@Name 'Value']";
        internal const string Section_Invalid_BadSyntax_ColonInsteadEqualSign = "Column[@Name:'Value']";
        internal const string Section_Invalid_BadSyntax_PropertiesSection_ParenthesesInsteadBrackets = "Column(@Name='Value')";
        internal const string Section_Invalid_BadSyntax_PropertySeparator_CommaInsteadAnd = "Table[@Name='Value', @Schema='Value']";
    }
}
