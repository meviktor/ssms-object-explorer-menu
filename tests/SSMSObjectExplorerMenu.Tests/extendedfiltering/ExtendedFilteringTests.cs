using SSMSObjectExplorerMenu.extendedfiltering;
using SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes;
using static SSMSObjectExplorerMenu.Constants;
using static SSMSObjectExplorerMenu.Tests.extendedfiltering.TestSamples;

namespace SSMSObjectExplorerMenu.Tests.extendedfiltering
{
    public class ExtendedFilteringTests
    {
        [Theory]
        #region TestCases
        // Positive test cases: Column
        [InlineData(true, ContextLevel.Column, Identifier_Valid)]
        [InlineData(true, ContextLevel.Column, Identifier_Valid_Digits)]
        [InlineData(true, ContextLevel.Column, Identifier_Valid_WithSpecialChars)]
        [InlineData(true, ContextLevel.Column, Identifier_Valid_Any)]
        // Positive test cases: Table
        [InlineData(true, ContextLevel.Table, Identifier_Valid, Identifier_Valid_Digits)]
        [InlineData(true, ContextLevel.Table, Identifier_Valid_Digits, Identifier_Valid)]
        [InlineData(true, ContextLevel.Table, Identifier_Valid_WithSpecialChars, Identifier_Valid)]
        [InlineData(true, ContextLevel.Table, Identifier_Valid_Any, Identifier_Valid)]
        [InlineData(true, ContextLevel.Table, Identifier_Valid, Identifier_Valid_Any)]
        [InlineData(true, ContextLevel.Table, Identifier_Valid_Any, Identifier_Valid_Any)]
        // Positive test cases: Database
        [InlineData(true, ContextLevel.Database, Identifier_Valid)]
        [InlineData(true, ContextLevel.Database, Identifier_Valid_Digits)]
        [InlineData(true, ContextLevel.Database, Identifier_Valid_WithSpecialChars)]
        [InlineData(true, ContextLevel.Database, Identifier_Valid_Any)]
        // Positive test cases: Server
        [InlineData(true, ContextLevel.Server, Identifier_Valid)]
        [InlineData(true, ContextLevel.Server, Identifier_Valid_Digits)]
        [InlineData(true, ContextLevel.Server, Identifier_Valid_WithSpecialChars)]
        [InlineData(true, ContextLevel.Server, Identifier_Valid_Any)]
        [InlineData(true, ContextLevel.Server, Identifier_ContainsDot)]
        [InlineData(true, ContextLevel.Server, Identifier_ContainsHyphen)]
        // Negative test cases: Column
        [InlineData(false, ContextLevel.Column, null)]
        [InlineData(false, ContextLevel.Column, Identifier_Invalid_Empty)]
        [InlineData(false, ContextLevel.Column, Identifier_Invalid_WhiteSpaceOnly)]
        [InlineData(false, ContextLevel.Column, Identifier_Invalid_StartsWithDigit)]
        [InlineData(false, ContextLevel.Column, Identifier_ContainsHyphen)]
        [InlineData(false, ContextLevel.Column, Identifier_Invalid_ContainsSpace)]
        [InlineData(false, ContextLevel.Column, Identifier_ContainsDot)]
        [InlineData(false, ContextLevel.Column, Identifier_Invalid_ContainsComma)]
        [InlineData(false, ContextLevel.Column, Identifier_Invalid_ContainsParentheses)]
        [InlineData(false, ContextLevel.Column, Identifier_Invalid_ContainsBracket)]
        [InlineData(false, ContextLevel.Column, Identifier_Invalid_ContainsNewLine)]
        [InlineData(false, ContextLevel.Column, Identifier_Invalid_ContainsTab)]
        // Negative test cases: Database (same rules as for columns)
        [InlineData(false, ContextLevel.Database, null)]
        [InlineData(false, ContextLevel.Database, Identifier_Invalid_Empty)]
        [InlineData(false, ContextLevel.Database, Identifier_Invalid_WhiteSpaceOnly)]
        [InlineData(false, ContextLevel.Database, Identifier_Invalid_StartsWithDigit)]
        [InlineData(false, ContextLevel.Database, Identifier_ContainsHyphen)]
        [InlineData(false, ContextLevel.Database, Identifier_Invalid_ContainsSpace)]
        [InlineData(false, ContextLevel.Database, Identifier_ContainsDot)]
        [InlineData(false, ContextLevel.Database, Identifier_Invalid_ContainsComma)]
        [InlineData(false, ContextLevel.Database, Identifier_Invalid_ContainsParentheses)]
        [InlineData(false, ContextLevel.Database, Identifier_Invalid_ContainsBracket)]
        [InlineData(false, ContextLevel.Database, Identifier_Invalid_ContainsNewLine)]
        [InlineData(false, ContextLevel.Database, Identifier_Invalid_ContainsTab)]
        // Negative test cases: Table (same rules as for columns)
        [InlineData(false, ContextLevel.Table, null, null)]
        [InlineData(false, ContextLevel.Table, null, Identifier_Valid_Digits)]
        [InlineData(false, ContextLevel.Table, Identifier_Invalid_Empty, Identifier_Valid_Digits)]
        [InlineData(false, ContextLevel.Table, Identifier_Valid, Identifier_Invalid_WhiteSpaceOnly)]
        [InlineData(false, ContextLevel.Table, Identifier_Invalid_StartsWithDigit, Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, Identifier_Valid, Identifier_ContainsHyphen)]
        [InlineData(false, ContextLevel.Table, Identifier_Invalid_ContainsSpace, Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, Identifier_Valid, Identifier_ContainsDot)]
        [InlineData(false, ContextLevel.Table, Identifier_Invalid_ContainsComma, Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, Identifier_Valid, Identifier_Invalid_ContainsParentheses)]
        [InlineData(false, ContextLevel.Table, Identifier_Invalid_ContainsBracket, Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, Identifier_Valid, Identifier_Invalid_ContainsNewLine)]
        [InlineData(false, ContextLevel.Table, Identifier_Invalid_ContainsTab, Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, Identifier_ContainsDot, Identifier_ContainsHyphen)]
        // Negative test cases: Server (dot and hyphen are allowed in server name)
        [InlineData(false, ContextLevel.Server, null)]
        [InlineData(false, ContextLevel.Server, Identifier_Invalid_Empty)]
        [InlineData(false, ContextLevel.Server, Identifier_Invalid_WhiteSpaceOnly)]
        [InlineData(false, ContextLevel.Server, Identifier_Invalid_StartsWithDigit)]
        [InlineData(false, ContextLevel.Server, Identifier_Invalid_ContainsSpace)]
        [InlineData(false, ContextLevel.Server, Identifier_Invalid_ContainsComma)]
        [InlineData(false, ContextLevel.Server, Identifier_Invalid_ContainsParentheses)]
        [InlineData(false, ContextLevel.Server, Identifier_Invalid_ContainsBracket)]
        [InlineData(false, ContextLevel.Server, Identifier_Invalid_ContainsNewLine)]
        [InlineData(false, ContextLevel.Server, Identifier_Invalid_ContainsTab)]
        #endregion
        // Positive & negative test cases with different values/identifiers (name, scheam) passed in the sections
        internal void Section_Identify_Test(bool shouldIdentify, ContextLevel sectionType, string? name, string? schema = null)
            => Section_Identify_Test_Core(shouldIdentify, ERROR_BUILD_FILTER_UNKNOWN_SECTION, sectionType, name, schema);

        [Theory]
        [InlineData(ERROR_BUILD_FILTER_COLUMN_NAME_TOO_LONG, ContextLevel.Column, Identifier_Invalid_LongerThan128Chars)]
        [InlineData(ERROR_BUILD_FILTER_DATABASE_NAME_TOO_LONG, ContextLevel.Database, Identifier_Invalid_LongerThan128Chars)]
        [InlineData(ERROR_BUILD_FILTER_SCHEMA_NAME_TOO_LONG, ContextLevel.Table, Identifier_Valid, Identifier_Invalid_LongerThan128Chars)]
        [InlineData(ERROR_BUILD_FILTER_TABLE_NAME_TOO_LONG, ContextLevel.Table, Identifier_Invalid_LongerThan128Chars, Identifier_Valid)]
        [InlineData(ERROR_BUILD_FILTER_SERVER_NAME_TOO_LONG, ContextLevel.Server, Identifier_Invalid_LongerThan128Chars)]
        // Section_Identify_Test, specific cases of too long identifiers (> 128 chars)
        internal void Section_Identify_Test_IdentifierTooLong(string expectedError, ContextLevel sectionType, string? name, string? schema = null)
             => Section_Identify_Test_Core(false, expectedError, sectionType, name, schema);

        [Theory]
        #region TestCases
        // Unknown section type
        [InlineData(Section_Invalid_Type_NotKnown)]
        // Properties section is not surrounded by []
        [InlineData(Section_Invalid_BadSyntax_PropertiesSection_ParenthesesInsteadBrackets)]
        // Property name not known
        [InlineData(Section_Invalid_Property_NotKnown)]
        // Property name not starting with '@'
        [InlineData(Section_Invalid_BadSyntax_MissingAt)]
        // Property value not surrounded by quotes
        [InlineData(Section_Invalid_BadSyntax_MissingQuotes)]
        // Property name-value separator is not '='
        [InlineData(Section_Invalid_BadSyntax_MissingEqualSign)]
        [InlineData(Section_Invalid_BadSyntax_ColonInsteadEqualSign)]
        #endregion
        // Negative test cases for the overall syntax of a single segment
        internal void Section_WithBadSyntax_Identify_Test(string? sectionWithBadSyntax)
        {
            // Act & assert
            var buildResult = ExtendedFilteringProperties.BuildFromNavigationContext(sectionWithBadSyntax, out var errors);

            Assert.Null(buildResult);
            Assert.Contains(ERROR_BUILD_FILTER_UNKNOWN_SECTION, errors);
        }

        [Theory]
        [InlineData(true, "CreationDate", "CreationDate")]
        [InlineData(true, "creationDate", "Creationdate")]
        [InlineData(false, "creationDate", "CreatedOn")]
        internal void Section_Column_Equality_Test(bool areEqual, string name1, string name2) => Assert.Equal(areEqual, AreEqual((name) => new Column(name), name1, name2));

        [Theory]
        [InlineData(true, "dbo", "customers", "dbo", "customers")]
        [InlineData(true, "dbo", "customers", "DBO", "customers")]
        [InlineData(true, "dbo", "customers", "DBO", "Customers")]
        [InlineData(true, "dbo", "customers", "dbo", "Customers")]
        [InlineData(false, "dbo", "customers", "myCompany", "customers")]
        [InlineData(false, "dbo", "customers", "dbo", "MyCustomers")]
        [InlineData(false, "dbo", "customers", "myCompany", "MyCustomers")]
        internal void Section_Table_Equality_Test(bool areEqual, string schema1, string name1, string schema2, string name2)
            => Assert.Equal(areEqual, AreEqual((schema, name) => new Table(name, schema), schema1, name1, schema2, name2));

        [Theory]
        [InlineData(true, "myDb", "myDb")]
        [InlineData(true, "mydb", "myDB")]
        [InlineData(false, "myDB", "myDatabase")]
        internal void Section_Database_Equality_Test(bool areEqual, string name1, string name2) => Assert.Equal(areEqual, AreEqual((name) => new Database(name), name1, name2));

        [Theory]
        [InlineData(true, "localhost", "localhost")]
        [InlineData(true, "localhost", "LOCALHOST")]
        [InlineData(false, "localhost", "127.0.0.1")]
        internal void Section_Server_Equality_Test(bool areEqual, string name1, string name2) => Assert.Equal(areEqual, AreEqual((name) => new Server(name), name1, name2));

        // TODO: check if we could exchange the the baked in strings with constants from the TestSamples class!
        [Theory]
        #region TestCases
        // Negative: sections are separating with anything but forward slash
        [InlineData(false, ERROR_BUILD_FILTER_UNKNOWN_SECTION, $"{NavContext_Server_Segment} {NavContext_Database_Segment}")]
        [InlineData(false, ERROR_BUILD_FILTER_UNKNOWN_SECTION, $"{NavContext_Server_Segment}\\{NavContext_Database_Segment}")]
        [InlineData(false, ERROR_BUILD_FILTER_UNKNOWN_SECTION, $"{NavContext_Server_Segment};{NavContext_Database_Segment}")]
        // Negative: sections ordering are invalid
        [InlineData(false, ERROR_BUILD_FILTER_INVALID_SECTION_ORDER, $"{NavContext_Database_Segment}/{NavContext_Server_Segment}")]
        [InlineData(false, ERROR_BUILD_FILTER_INVALID_SECTION_ORDER, $"{NavContext_Column_Segment}/{NavContext_Server_Segment}")]
        [InlineData(false, ERROR_BUILD_FILTER_INVALID_SECTION_ORDER, $"{NavContext_Table_Segment}/{NavContext_Database_Segment}")]
        // Negative: duplicated sections
        [InlineData(false, ERROR_BUILD_FILTER_DUPLICATED_SECTION, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/Database[@Name='OtherDatabase']")]
        [InlineData(false, ERROR_BUILD_FILTER_DUPLICATED_SECTION, $"{NavContext_Server_Segment}/Server[@Name='OtherServer']/{NavContext_Database_Segment}")]
        [InlineData(false, ERROR_BUILD_FILTER_DUPLICATED_SECTION, $"{NavContext_Server_Segment}/{NavContext_Column_Segment}/Column[@Name='OtherColumn']")]
        [InlineData(false, ERROR_BUILD_FILTER_DUPLICATED_SECTION, $"{NavContext_Database_Segment}/{NavContext_Table_Segment}/Table[@Name='*' and @Schema='*']")]
        // Positive: string is null/empty/whitespace-only - no errors, an empty, "dummy" filter is returned
        [InlineData(true, null, null)]
        [InlineData(true, null, "")]
        [InlineData(true, null, "\t\v\r\n")]
        // Positive: string with all sections, omitting sections but the ordering is still right
        [InlineData(true, null, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/{NavContext_Table_Segment}/{NavContext_Column_Segment}")]
        [InlineData(true, null, $"{NavContext_Database_Segment}/{NavContext_Table_Segment}/{NavContext_Column_Segment}")]
        [InlineData(true, null, $"{NavContext_Server_Segment}/{NavContext_Table_Segment}/{NavContext_Column_Segment}")]
        [InlineData(true, null, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/{NavContext_Column_Segment}")]
        [InlineData(true, null, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/{NavContext_Table_Segment}")]
        // Positive: "empty sections" (creating accidentally by placing double forward slashes or starting with a forward slash) will not cause an error (they are ignored)
        [InlineData(true, null, $"{NavContext_Database_Segment}//{NavContext_Table_Segment}/{NavContext_Column_Segment}")]
        [InlineData(true, null, $"/{NavContext_Database_Segment}/{NavContext_Column_Segment}")]
        #endregion
        // Test cases of whole filter/navigation context strings, not only single sections
        internal void NavigationContextString_Validation_Test(bool shouldBeValid, string? expectedError, string? navigationContext)
            => Build_Filter_Test(shouldBeValid, expectedError, navigationContext);

        [Theory]
        #region TestCases
        // Applicable contexts
        [InlineData(true, NavContext_StringFull_ContextLevel_Column, Column_Context)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Column, Table_Context, ERROR_FILTER_VALIDATE_CONTEXT_FILTER_TARGETS_LOW_CONTEXT)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Column, Database_Context, ERROR_FILTER_VALIDATE_CONTEXT_FILTER_TARGETS_LOW_CONTEXT)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Column, Server_Context, ERROR_FILTER_VALIDATE_CONTEXT_FILTER_TARGETS_LOW_CONTEXT)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Table, Column_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Table, Table_Context)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Table, Database_Context, ERROR_FILTER_VALIDATE_CONTEXT_FILTER_TARGETS_LOW_CONTEXT)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Table, Server_Context, ERROR_FILTER_VALIDATE_CONTEXT_FILTER_TARGETS_LOW_CONTEXT)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Database, Column_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Database, Table_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Database, Database_Context)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Database, Server_Context, ERROR_FILTER_VALIDATE_CONTEXT_FILTER_TARGETS_LOW_CONTEXT)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Server, Column_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Server, Table_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Server, Database_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Server, Server_Context)]
        // Not applicable contexts
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/DatabasesFolder", ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/Database/UserTablesFolder", ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/Database/View", ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/Database/StoredProcedure", ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/Database/StoredProceduresFolder", ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/JobServer", ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/JobServer/JobsFolder", ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/JobServer/Job", ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE)]
        // Strings denominate no context kind
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "JustARandomStringNoContext", ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Database/View", ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "StoredProceduresFolder", ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE)]
        #endregion
        // Testing if a valid navigation context string with a specific context level is accepted as a filter for a menu item with a specific context level
        internal void ValidateForContext_Test(bool shouldBeValid, string navigationContext, string targetContextLevel, string? expectedError = null)
        {
            // Act & assert
            var filter = ExtendedFilteringProperties.BuildFromNavigationContext(navigationContext, out var _);
            var isValidForContext = filter.TryValidateForContext(targetContextLevel, out var errors);

            Assert.Equal(shouldBeValid, isValidForContext);
            if (!shouldBeValid)
                Assert.Contains(expectedError, errors);
        }

        [Theory]
        #region TestCases
        // Context: Server
        [InlineData(NavContext_Server_Segment, ContextLevel.Server)]
        // Context: Database
        [InlineData(NavContext_Database_Segment, ContextLevel.Database)]
        [InlineData($"{NavContext_Server_Segment}/{NavContext_Database_Segment}", ContextLevel.Database)]
        // Context: Table
        [InlineData(NavContext_Table_Segment, ContextLevel.Table)]
        // Context: Table, with 2 segments
        [InlineData($"{NavContext_Database_Segment}/{NavContext_Table_Segment}", ContextLevel.Table)]
        [InlineData($"{NavContext_Server_Segment}/{NavContext_Table_Segment}", ContextLevel.Table)]
        [InlineData($"{NavContext_Server_Segment}/{NavContext_Database_Segment}/{NavContext_Table_Segment}", ContextLevel.Table)]
        // Context: Column
        [InlineData(NavContext_Column_Segment, ContextLevel.Column)]
        // Context: Column, with 2 segments
        [InlineData($"{NavContext_Table_Segment}/{NavContext_Column_Segment}", ContextLevel.Column)]
        [InlineData($"{NavContext_Database_Segment}/{NavContext_Column_Segment}", ContextLevel.Column)]
        [InlineData($"{NavContext_Server_Segment}/{NavContext_Column_Segment}", ContextLevel.Column)]
        // Context: Column, with 3 segments
        [InlineData($"{NavContext_Server_Segment}/{NavContext_Database_Segment}/{NavContext_Column_Segment}", ContextLevel.Column)]
        [InlineData($"{NavContext_Server_Segment}/{NavContext_Table_Segment}/{NavContext_Column_Segment}", ContextLevel.Column)]
        [InlineData($"{NavContext_Database_Segment}/{NavContext_Table_Segment}/{NavContext_Column_Segment}", ContextLevel.Column)]
        // Context: Column, 4 segments
        [InlineData($"{NavContext_Server_Segment}/{NavContext_Database_Segment}/{NavContext_Table_Segment}/{NavContext_Column_Segment}", ContextLevel.Column)]
        #endregion
        // Building filters from valid navigation context strings and test if property values are set as expected
        internal void BuildFromNavigationContext_NotEmptyFilters_Test(string navigationContext, ContextLevel context)
        {
            var filter = ExtendedFilteringProperties.BuildFromNavigationContext(navigationContext, out var _);

            Assert.NotNull(filter);

            switch (context)
            {
                case ContextLevel.Column:
                    Assert.Multiple(
                        () => Assert.False(filter.IsEmpty),
                        () => Assert.True(filter.Context is ContextLevel.Column),
                        () => Assert.True(filter.Column.IsActiveFilter),
                        () => Assert.NotNull(filter.Table),
                        () => Assert.NotNull(filter.Database),
                        () => Assert.NotNull(filter.Server));
                    break;
                case ContextLevel.Table:
                    Assert.Multiple(
                        () => Assert.False(filter.IsEmpty),
                        () => Assert.True(filter.Context is ContextLevel.Table),
                        () => Assert.Null(filter.Column),
                        () => Assert.True(filter.Table.IsActiveFilter),
                        () => Assert.NotNull(filter.Database),
                        () => Assert.NotNull(filter.Server));
                    break;
                case ContextLevel.Database:
                    Assert.Multiple(
                        () => Assert.False(filter.IsEmpty),
                        () => Assert.True(filter.Context is ContextLevel.Database),
                        () => Assert.Null(filter.Column),
                        () => Assert.Null(filter.Table),
                        () => Assert.True(filter.Database.IsActiveFilter),
                        () => Assert.NotNull(filter.Server));
                    break;
                case ContextLevel.Server:
                    Assert.Multiple(
                        () => Assert.False(filter.IsEmpty),
                        () => Assert.True(filter.Context is ContextLevel.Server),
                        () => Assert.Null(filter.Column),
                        () => Assert.Null(filter.Table),
                        () => Assert.Null(filter.Database),
                        () => Assert.True(filter.Server.IsActiveFilter));
                    break;
                default:
                    Assert.Fail($"Implementation missisng for context type: {context}.");
                    break;
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("\t\v\r\n")]
        [InlineData(NavContext_AnySegment_Column)]
        [InlineData(NavContext_AnySegment_Table)]
        [InlineData(NavContext_AnySegment_Database)]
        [InlineData(NavContext_AnySegment_Server)]
        [InlineData($"{NavContext_AnySegment_Table}/{NavContext_AnySegment_Column}")]
        [InlineData($"{NavContext_AnySegment_Database}/{NavContext_AnySegment_Table}")]
        [InlineData($"{NavContext_AnySegment_Server}/{NavContext_AnySegment_Database}")]
        [InlineData($"{NavContext_AnySegment_Server}/{NavContext_AnySegment_Database}/{NavContext_AnySegment_Table}/{NavContext_AnySegment_Column}")]
        // Building filters from "Any"/empty navigation context strings and test if property values are set as expected
        internal void BuildFromNavigationContext_EmptyFilters_Test(string? navigationContext)
        {
            var filter = ExtendedFilteringProperties.BuildFromNavigationContext(navigationContext, out var _);

            Assert.True(filter.IsEmpty);
            Assert.Null(filter.Context);
        }

        [Theory]
        // null/empty/whitespace-only filters
        [InlineData(true, null)]
        [InlineData(true, "")]
        [InlineData(true, "\t\r\n\v")]
        // Sinlge "Any" segments: they're not filtering (like we haven't defined anything)
        [InlineData(true, $"{NavContext_AnySegment_Column}")]
        [InlineData(true, $"{NavContext_AnySegment_Database}")]
        // Combining filtering Column segment with "Any" segment(s)
        [InlineData(true, $"{NavContext_AnySegment_Database}/{NavContext_Column_Segment}")]
        [InlineData(true, $"{NavContext_AnySegment_Server}/{NavContext_AnySegment_Table}/{NavContext_Column_Segment}")]
        // Full filter matching
        [InlineData(true, NavContext_StringFull_ContextLevel_Column)]
        // Examples for case-insensitive match
        [InlineData(true, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/{NavContext_Table_Segment}/Column[@Name='PRICE']")]
        [InlineData(true, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/Table[@Name='SALES' and @Schema='dbo']/{NavContext_Column_Segment}")]
        [InlineData(true, $"{NavContext_Server_Segment}/Database[@Name='mycompany']/{NavContext_Table_Segment}/{NavContext_Column_Segment}")]
        [InlineData(true, $"Server[@Name='LOCALHOST']/{NavContext_Database_Segment}/{NavContext_Table_Segment}/{NavContext_Column_Segment}")]
        // Tables: wildcarding only the schema or the name
        [InlineData(true, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/Table[@Name='*' and @Schema='{NavContext_SchemaName}']/{NavContext_Column_Segment}")]
        [InlineData(true, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/Table[@Name='{NavContext_TableName}' and @Schema='*']/{NavContext_Column_Segment}")]
        // Filtering fails
        [InlineData(false, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/{NavContext_Table_Segment}/Column[@Name='WontMatch']")] // Column does not match
        [InlineData(false, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/Table[@Name='{NavContext_TableName}' and @Schema='WontMatch']/{NavContext_Column_Segment}")] // Table schema does not match
        [InlineData(false, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/Table[@Name='WontMatch' and @Schema='{NavContext_SchemaName}']/{NavContext_Column_Segment}")] // Table name does not match
        [InlineData(false, $"{NavContext_Server_Segment}/{NavContext_Database_Segment}/Table[@Name='WontMatch' and @Schema='WontMatch']/{NavContext_Column_Segment}")] // Table does not match
        [InlineData(false, $"{NavContext_Server_Segment}/Database[@Name='WontMatch']/{NavContext_Table_Segment}/{NavContext_Column_Segment}")] // Database does not match
        [InlineData(false, $"Server[@Name='WontMatch']/{NavContext_Database_Segment}/{NavContext_Table_Segment}/{NavContext_Column_Segment}")] // Server does not match
        internal void ApplyFiltering_Test(bool isAllowedByFilter, string? strFilter)
        {
            // Arrange
            // Node representing a column in the SSMS Object Explorer Tree
            // Why column - filters with all target contexts can be used for columns
            var node = ExtendedFilteringProperties.BuildFromNavigationContext(NavContext_StringFull_ContextLevel_Column, out var _);
            // Filter associated to a MenuItem
            var filter = ExtendedFilteringProperties.BuildFromNavigationContext(strFilter, out var _);

            // Action & Assert
            Assert.Equal(isAllowedByFilter, node.ApplyFiltering(filter));
        }

        private static bool AreEqual<TSection>(Func<string, TSection> createSection, string name1, string name2) where TSection : NameProperty
            => createSection(name1) == createSection(name2);

        private static bool AreEqual<TSection>(Func<string, string, TSection> createSection, string schema1, string name1, string schema2, string name2) where TSection : NameSchemaProperties
            => createSection(schema1, name1) == createSection(schema2, name2);

        private static void Section_Identify_Test_Core(bool shouldIdentify, string? expectedError, ContextLevel sectionType, string? name, string? schema = null)
        {
            // Arrange
            var section = GenerateSection(sectionType, name, schema);
            // Act & assert
            Build_Filter_Test(shouldIdentify, expectedError, section);
        }

        private static void Build_Filter_Test(bool shouldIdentify, string? expectedError, string? filterString)
        {
            var filter = ExtendedFilteringProperties.BuildFromNavigationContext(filterString, out var errors);
            var isIdentified = filter != null && !errors.Any();

            Assert.Equal(shouldIdentify, isIdentified);
            if (!shouldIdentify)
                Assert.Contains(expectedError, errors);
        }

        private static string GenerateSection(ContextLevel sectionType, string? name, string? schema = null)
        {
            if(sectionType != ContextLevel.Table && schema is not null)
                throw new ArgumentException($"Schema is not applicable for context level '{sectionType}'.", nameof(schema));

            return sectionType switch
            {
                ContextLevel.Server => $"Server[@Name='{name}']",
                ContextLevel.Database => $"Database[@Name='{name}']",
                ContextLevel.Table => $"Table[@Name='{name}' and @Schema='{schema}']",
                ContextLevel.Column => $"Column[@Name='{name}']",
                _ => throw new ArgumentException($"Unknown context level '{sectionType}'.", nameof(sectionType))
            };
        } 
    }
}
