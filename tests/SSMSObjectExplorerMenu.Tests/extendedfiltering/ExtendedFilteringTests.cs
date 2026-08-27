using SSMSObjectExplorerMenu.extendedfiltering;
using SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes;
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
        [InlineData(false, ContextLevel.Column, Identifier_Invalid_LongerThan128Chars)]
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
        [InlineData(false, ContextLevel.Database, Identifier_Invalid_LongerThan128Chars)]
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
        [InlineData(false, ContextLevel.Table, Identifier_Valid, Identifier_Invalid_LongerThan128Chars)]
        [InlineData(false, ContextLevel.Table, Identifier_Invalid_LongerThan128Chars, Identifier_Invalid_LongerThan128Chars)]
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
        [InlineData(false, ContextLevel.Server, Identifier_Invalid_LongerThan128Chars)]
        #endregion
        // Positive & negative test cases with different values/identifiers (name, scheam) passed in the sections
        internal void Section_Identify_Test(bool shouldIdentify, ContextLevel sectionType, string? name, string? schema = null)
        {
            // Arrange
            var section = GenerateSection(sectionType, name, schema);
            var action = () => ExtendedFilteringProperties.BuildFromNavigationContext(section);
            // Act & assert
            var isIdentified = Record.Exception(action) is null;
            // TODO: in case the error messages will be extracted into string constants, we could try assert also the message in the exception to be more accurate!
            Assert.Equal(shouldIdentify, isIdentified);
        }

        [Theory]
        #region TestCases
        // Section is null/empty/whitespace-only
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" \t\v\r\n")]
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
            // Arrange
            var action = () => ExtendedFilteringProperties.BuildFromNavigationContext(sectionWithBadSyntax);
            // Act & assert
            // TODO: in case the error messages will be extracted into string constants, we could try assert also the message in the exception to be more accurate!
            Assert.Throws<ArgumentException>(action);
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

        [Theory]
        #region TestCases
        // Negative: string is null/empty/whitespace-only
        [InlineData(false, null)]
        [InlineData(false, "")]
        [InlineData(false, "\t\v\r\n")]
        // Negative: sections are separating with anything but forward slash
        [InlineData(false, "Server[@Name='localhost'] Database[@Name='BusinessResults']")]
        [InlineData(false, "Server[@Name='localhost']\\Database[@Name='BusinessResults']")]
        [InlineData(false, "Server[@Name='localhost'];Database[@Name='BusinessResults']")]
        // Negative: sections ordering are invalid
        [InlineData(false, "Database[@Name='BusinessResults']/Server[@Name='localhost']")]
        [InlineData(false, "Column[@Name='id']/Server[@Name='localhost']")]
        [InlineData(false, "Table[@Name='Sales' and @Schema='FirstCompany']/Database[@Name='BusinessResults']")]
        // Negative: duplicated sections
        [InlineData(false, "Server[@Name='localhost']/Database[@Name='BusinessResults']/Database[@Name='Statistics']")]
        [InlineData(false, "Server[@Name='localhost']/Server[@Name='127.0.0.1']/Database[@Name='Statistics']")]
        [InlineData(false, "Server[@Name='localhost']/Column[@Name='Id']/Column[@Name='FirstName']")]
        [InlineData(false, "Database[@Name='BusinessResults']/Table[@Name='Sales' and @Schema='FirstCompany']/Table[@Name='*' and @Schema='*']")]
        // Positive: string with all sections, omitting sections but the ordering is still right
        [InlineData(true, "Server[@Name='localhost']/Database[@Name='BusinessResults']/Table[@Name='Sales' and @Schema='FirstCompany']/Column[@Name='id']")]
        [InlineData(true, "Database[@Name='BusinessResults']/Table[@Name='Sales' and @Schema='FirstCompany']/Column[@Name='id']")]
        [InlineData(true, "Server[@Name='localhost']/Table[@Name='Sales' and @Schema='FirstCompany']/Column[@Name='id']")]
        [InlineData(true, "Server[@Name='localhost']/Database[@Name='BusinessResults']/Column[@Name='Amount']")]
        [InlineData(true, "Server[@Name='localhost']/Database[@Name='BusinessResults']/Table[@Name='Sales' and @Schema='FirstCompany']")]
        // Positive: "empty sections" (creating accidentally by placing double forward slashes or starting with a forward slash) will not cause an error (they are ignored)
        [InlineData(true, "Database[@Name='BusinessResults']//Table[@Name='Sales' and @Schema='FirstCompany']/Column[@Name='id']")]
        [InlineData(true, "/Database[@Name='BusinessResults']/Column[@Name='SoldQty']")]
        #endregion
        // Test cases of whole filter/navigation context strings, not only single sections
        internal void NavigationContextString_Validation_Test(bool shouldBeValid, string? navigationContext)
        {
            var action = () => ExtendedFilteringProperties.BuildFromNavigationContext(navigationContext);
            // Act & assert
            var isValid = Record.Exception(action) is null;
            // TODO: in case the error messages will be extracted into string constants, we could try assert also the message in the exception to be more accurate!
            Assert.Equal(shouldBeValid, isValid);
        }

        [Theory]
        #region TestCases
        // Applicable contexts
        [InlineData(true, NavContext_StringFull_ContextLevel_Column, Constants.Column_Context)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Column, Constants.Table_Context)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Column, Constants.Database_Context)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Column, Constants.Server_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Table, Constants.Column_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Table, Constants.Table_Context)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Table, Constants.Database_Context)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Table, Constants.Server_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Database, Constants.Column_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Database, Constants.Table_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Database, Constants.Database_Context)]
        [InlineData(false, NavContext_StringFull_ContextLevel_Database, Constants.Server_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Server, Constants.Column_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Server, Constants.Table_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Server, Constants.Database_Context)]
        [InlineData(true, NavContext_StringFull_ContextLevel_Server, Constants.Server_Context)]
        // Not applicable contexts
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/DatabasesFolder")]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/Database/UserTablesFolder")]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/Database/View")]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/Database/StoredProcedure")]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/Database/StoredProceduresFolder")]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/JobServer")]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/JobServer/JobsFolder")]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Server/JobServer/Job")]
        // Strings denominate no context kind
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "JustARandomStringNoContext")]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "Database/View")]
        [InlineData(false, NavContext_StringFull_ContextLevel_Server, "StoredProceduresFolder")]
        #endregion
        // Testing if a valid navigation context string with a specific context level is accepted as a filter for a menu item with a specific context level
        internal void ValidateForContext_Test(bool shouldBeValid, string navigationContext, string menuItemContextLevel)
        {
            // Act & assert
            var validationResult = ExtendedFilteringProperties.ValidateForContext(navigationContext, menuItemContextLevel);

            // TODO: in case the error messages will be extracted into string constants, we could try assert also the message in the exception to be more accurate!
            // TODO: check on the returned error as well!
            Assert.Equal(shouldBeValid, validationResult.IsValid);
        }

        // TODO: null/empty/whitespace for ValidateForContext - in a separate case (also modify the original method)!

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
        internal void BuildFromNavigationContext_NotEmptyFilters_Test(string navigationContext, ContextLevel context)
        {
            var filter = ExtendedFilteringProperties.BuildFromNavigationContext(navigationContext);
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
        internal void BuildFromNavigationContext_EmptyFilters_Test(string? navigationContext)
        {
            var filter = ExtendedFilteringProperties.BuildFromNavigationContext(navigationContext);

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
            var node = ExtendedFilteringProperties.BuildFromNavigationContext(NavContext_StringFull_ContextLevel_Column);
            // Filter associated to a MenuItem
            var filter = ExtendedFilteringProperties.BuildFromNavigationContext(strFilter);

            // Assert
            Assert.Equal(isAllowedByFilter, node.ApplyFiltering(filter));
        }

        private static bool AreEqual<TSection>(Func<string, TSection> createSection, string name1, string name2) where TSection : NameProperty
            => createSection(name1) == createSection(name2);

        private static bool AreEqual<TSection>(Func<string, string, TSection> createSection, string schema1, string name1, string schema2, string name2) where TSection : NameSchemaProperties
            => createSection(schema1, name1) == createSection(schema2, name2);

        private static string GenerateSection(ContextLevel sectionType, string? name, string? schema = null)
        {
            if(sectionType != ContextLevel.Table && schema is not null)
                throw new ArgumentException($"Schema is not applicable for context level '{sectionType}'.", nameof(schema));

            //if (sectionType == ContextLevel.Table && schema is null)
            //    throw new ArgumentException($"Schema must be provided for context level '{sectionType}'.", nameof(schema));

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

    //[Flags]
    //internal enum NavContextStringSegment
    //{
    //    None = 0,
    //    Column = 1,
    //    Table = 2,
    //    Database = 4,
    //    Server = 8,
    //    All = Column | Table | Database | Server
    //}

    //internal static class UsingSegmentsExtensions
    //{
    //    internal static bool Contains(this NavContextStringSegment segments, NavContextStringSegment segmentToFind) => (segments & segmentToFind) == segmentToFind;
    //}
}
