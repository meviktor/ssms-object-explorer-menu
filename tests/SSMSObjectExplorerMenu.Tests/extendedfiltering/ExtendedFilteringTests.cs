using SSMSObjectExplorerMenu.extendedfiltering;
using SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes;

namespace SSMSObjectExplorerMenu.Tests.extendedfiltering
{
    public class ExtendedFilteringTests
    {
        [Theory]
        #region TestCases
        // Positive test cases: Column
        [InlineData(true, ContextLevel.Column, TestSamples.Identifier_Valid)]
        [InlineData(true, ContextLevel.Column, TestSamples.Identifier_Valid_Digits)]
        [InlineData(true, ContextLevel.Column, TestSamples.Identifier_Valid_WithSpecialChars)]
        [InlineData(true, ContextLevel.Column, TestSamples.Identifier_Valid_Any)]
        // Positive test cases: Table
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Valid_Digits)]
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid_Digits, TestSamples.Identifier_Valid)]
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid_WithSpecialChars, TestSamples.Identifier_Valid)]
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid_Any, TestSamples.Identifier_Valid)]
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Valid_Any)]
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid_Any, TestSamples.Identifier_Valid_Any)]
        // Positive test cases: Database
        [InlineData(true, ContextLevel.Database, TestSamples.Identifier_Valid)]
        [InlineData(true, ContextLevel.Database, TestSamples.Identifier_Valid_Digits)]
        [InlineData(true, ContextLevel.Database, TestSamples.Identifier_Valid_WithSpecialChars)]
        [InlineData(true, ContextLevel.Database, TestSamples.Identifier_Valid_Any)]
        // Positive test cases: Server
        [InlineData(true, ContextLevel.Server, TestSamples.Identifier_Valid)]
        [InlineData(true, ContextLevel.Server, TestSamples.Identifier_Valid_Digits)]
        [InlineData(true, ContextLevel.Server, TestSamples.Identifier_Valid_WithSpecialChars)]
        [InlineData(true, ContextLevel.Server, TestSamples.Identifier_Valid_Any)]
        [InlineData(true, ContextLevel.Server, TestSamples.Identifier_ContainsDot)]
        [InlineData(true, ContextLevel.Server, TestSamples.Identifier_ContainsHyphen)]
        // Negative test cases: Column
        [InlineData(false, ContextLevel.Column, null)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_Empty)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_WhiteSpaceOnly)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_StartsWithDigit)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_ContainsHyphen)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsSpace)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_ContainsDot)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsComma)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsParentheses)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsBracket)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsNewLine)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsTab)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_LongerThan128Chars)]
        // Negative test cases: Database (same rules as for columns)
        [InlineData(false, ContextLevel.Database, null)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_Invalid_Empty)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_Invalid_WhiteSpaceOnly)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_Invalid_StartsWithDigit)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_ContainsHyphen)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_Invalid_ContainsSpace)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_ContainsDot)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_Invalid_ContainsComma)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_Invalid_ContainsParentheses)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_Invalid_ContainsBracket)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_Invalid_ContainsNewLine)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_Invalid_ContainsTab)]
        [InlineData(false, ContextLevel.Database, TestSamples.Identifier_Invalid_LongerThan128Chars)]
        // Negative test cases: Table (same rules as for columns)
        [InlineData(false, ContextLevel.Table, null, null)]
        [InlineData(false, ContextLevel.Table, null, TestSamples.Identifier_Valid_Digits)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_Empty, TestSamples.Identifier_Valid_Digits)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Invalid_WhiteSpaceOnly)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_StartsWithDigit, TestSamples.Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_ContainsHyphen)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_ContainsSpace, TestSamples.Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_ContainsDot)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_ContainsComma, TestSamples.Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Invalid_ContainsParentheses)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_ContainsBracket, TestSamples.Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Invalid_ContainsNewLine)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_ContainsTab, TestSamples.Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Invalid_LongerThan128Chars)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_LongerThan128Chars, TestSamples.Identifier_Invalid_LongerThan128Chars)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_ContainsDot, TestSamples.Identifier_ContainsHyphen)]
        // Negative test cases: Server (dot and hyphen are allowed in server name)
        [InlineData(false, ContextLevel.Server, null)]
        [InlineData(false, ContextLevel.Server, TestSamples.Identifier_Invalid_Empty)]
        [InlineData(false, ContextLevel.Server, TestSamples.Identifier_Invalid_WhiteSpaceOnly)]
        [InlineData(false, ContextLevel.Server, TestSamples.Identifier_Invalid_StartsWithDigit)]
        [InlineData(false, ContextLevel.Server, TestSamples.Identifier_Invalid_ContainsSpace)]
        [InlineData(false, ContextLevel.Server, TestSamples.Identifier_Invalid_ContainsComma)]
        [InlineData(false, ContextLevel.Server, TestSamples.Identifier_Invalid_ContainsParentheses)]
        [InlineData(false, ContextLevel.Server, TestSamples.Identifier_Invalid_ContainsBracket)]
        [InlineData(false, ContextLevel.Server, TestSamples.Identifier_Invalid_ContainsNewLine)]
        [InlineData(false, ContextLevel.Server, TestSamples.Identifier_Invalid_ContainsTab)]
        [InlineData(false, ContextLevel.Server, TestSamples.Identifier_Invalid_LongerThan128Chars)]
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
        [InlineData(TestSamples.Section_Invalid_Type_NotKnown)]
        // Properties section is not surrounded by []
        [InlineData(TestSamples.Section_Invalid_BadSyntax_PropertiesSection_ParenthesesInsteadBrackets)]
        // Property name not known
        [InlineData(TestSamples.Section_Invalid_Property_NotKnown)]
        // Property name not starting with '@'
        [InlineData(TestSamples.Section_Invalid_BadSyntax_MissingAt)]
        // Property value not surrounded by quotes
        [InlineData(TestSamples.Section_Invalid_BadSyntax_MissingQuotes)]
        // Property name-value separator is not '='
        [InlineData(TestSamples.Section_Invalid_BadSyntax_MissingEqualSign)]
        [InlineData(TestSamples.Section_Invalid_BadSyntax_ColonInsteadEqualSign)]
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
}
