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

        // Navigation context string segments
        internal const string NavContext_ColumnName = "Price";
        internal const string NavContext_TableName = "Sales";
        internal const string NavContext_SchemaName = "dbo";
        internal const string NavContext_DatabaseName = "MyCompany";
        internal const string NavContext_ServerName = "localhost";
        internal const string NavContext_Column_Segment = $"Column[@Name='{NavContext_ColumnName}']";
        internal const string NavContext_Table_Segment = $"Table[@Name='{NavContext_TableName}' and @Schema='{NavContext_SchemaName}']";
        internal const string NavContext_Database_Segment = $"Database[@Name='{NavContext_DatabaseName}']";
        internal const string NavContext_Server_Segment = $"Server[@Name='{NavContext_ServerName}']";

        // Navigation context string "Any" segments
        internal const string NavContext_AnySegment_Column = "Column[@Name='*']";
        internal const string NavContext_AnySegment_Table = "Table[@Name='*' and @Schema='*']";
        internal const string NavContext_AnySegment_Table_AnyName = "Table[@Name='*' and @Schema='dbo']";
        internal const string NavContext_AnySegment_Table_AnySchema = "Table[@Name='Sales' and @Schema='*']";
        internal const string NavContext_AnySegment_Database = "Database[@Name='*']";
        internal const string NavContext_AnySegment_Server = "Server[@Name='*']";

        // Navigation context strings
        internal const string NavContext_StringFull_ContextLevel_Column = $"{NavContext_StringFull_ContextLevel_Table}/{NavContext_Column_Segment}";
        internal const string NavContext_StringFull_ContextLevel_Table = $"{NavContext_StringFull_ContextLevel_Database}/{NavContext_Table_Segment}";
        internal const string NavContext_StringFull_ContextLevel_Database = $"{NavContext_StringFull_ContextLevel_Server}/{NavContext_Database_Segment}";
        internal const string NavContext_StringFull_ContextLevel_Server = NavContext_Server_Segment;
    }
}
