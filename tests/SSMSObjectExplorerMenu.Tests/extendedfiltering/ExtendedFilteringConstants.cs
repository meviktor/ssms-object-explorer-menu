using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;

namespace SSMSObjectExplorerMenu.Tests.extendedfiltering
{
    internal static class TestSamples
    {
        // Valid idenifiers for section properties (Name, Schema)
        internal const string Identifier_Valid = "Id";
        internal const string Identifier_Valid_Digits = "Id2";
        internal const string Identifier_Valid_WithSpecialChars = "@Identifier_Name#";
        internal const string Identifier_Valid_Any = $"{Wildcard_Any}";
        // Invalid idenifiers for section properties (Name, Schema)
        internal const string Identifier_Invalid_Empty = "";
        internal const string Identifier_Invalid_WhiteSpaceOnly = " ";
        internal const string Identifier_Invalid_StartsWithDigit = "1Id";
        internal const string Identifier_Invalid_ContainsHyphen = "order-id";
        internal const string Identifier_Invalid_ContainsSpace = "my column";
        internal const string Identifier_Invalid_ContainsDot = "a.b";
        internal const string Identifier_Invalid_ContainsComma = "price,vat";
        internal const string Identifier_Invalid_ContainsParentheses = "col(name)";
        internal const string Identifier_Invalid_ContainsBracket = "col[name";
        internal const string Identifier_Invalid_ContainsNewLine = "Id\nName";
        internal const string Identifier_Invalid_ContainsTab = "Id\tName";
        internal const string Identifier_Invalid_LongerThan128Chars = "@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789@123456789";
        // Invalid section samples
        internal const string Section_Invalid_Type_NotKnown = "NotKnownSection[@Name='Unknown']";
        internal const string Section_Invalid_Property_NotKnown = "NotKnownSection[@Unkown='Value']";
        internal const string Section_Invalid_BadSyntax_MissingAt = "Column[Name='Value']";
        internal const string Section_Invalid_BadSyntax_MissingQuotes = "Column[@Name=Value]";
        internal const string Section_Invalid_BadSyntax_MissingEqualSign = "Column[@Name 'Value']";
        internal const string Section_Invalid_BadSyntax_ColonInsteadEqualSign = "Column[@Name:'Value']";
    }
}
