using SSMSObjectExplorerMenu.extendedfiltering;

namespace SSMSObjectExplorerMenu.Tests.extendedfiltering
{
    public class ExtendedFilteringTests
    {
        [Theory]
        #region TestCases
        [InlineData(true, ContextLevel.Column, TestSamples.Identifier_Valid)]
        [InlineData(true, ContextLevel.Column, TestSamples.Identifier_Valid_Digits)]
        [InlineData(true, ContextLevel.Column, TestSamples.Identifier_Valid_WithSpecialChars)]
        [InlineData(true, ContextLevel.Column, TestSamples.Identifier_Valid_Any)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_Empty)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_WhiteSpaceOnly)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_StartsWithDigit)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsHyphen)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsSpace)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsDot)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsComma)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsParentheses)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsBracket)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsNewLine)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_ContainsTab)]
        [InlineData(false, ContextLevel.Column, TestSamples.Identifier_Invalid_LongerThan128Chars)]
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Valid_Digits)]
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid_Digits, TestSamples.Identifier_Valid)]
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid_WithSpecialChars, TestSamples.Identifier_Valid)]
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid_Any, TestSamples.Identifier_Valid)]
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Valid_Any)]
        [InlineData(true, ContextLevel.Table, TestSamples.Identifier_Valid_Any, TestSamples.Identifier_Valid_Any)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_Empty, TestSamples.Identifier_Valid_Digits)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Invalid_WhiteSpaceOnly)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_StartsWithDigit, TestSamples.Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Invalid_ContainsHyphen)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_ContainsSpace, TestSamples.Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Invalid_ContainsDot)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_ContainsComma, TestSamples.Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Invalid_ContainsParentheses)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_ContainsBracket, TestSamples.Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Invalid_ContainsNewLine)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_ContainsTab, TestSamples.Identifier_Valid)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Valid, TestSamples.Identifier_Invalid_LongerThan128Chars)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_LongerThan128Chars, TestSamples.Identifier_Invalid_LongerThan128Chars)]
        [InlineData(false, ContextLevel.Table, TestSamples.Identifier_Invalid_ContainsDot, TestSamples.Identifier_Invalid_ContainsHyphen)]
        #endregion
        internal void BuildFromNavigationConext_IdentifySingleSection_Identifiers_Test(bool shouldIdentify, ContextLevel sectionType, string name, string? schema = null)
        {
            // Arrange
            var section = GenerateSection(sectionType, name, schema);
            var action = () => ExtendedFilteringProperties.BuildFromNavigationContext(section);
            // Act
            var exception = Record.Exception(action) as ArgumentException;
            var isIdentified = exception is null;
            // Assert
            Assert.Equal(shouldIdentify, isIdentified);
        }

        [Theory]
        #region TestCases
        [InlineData(TestSamples.Section_Invalid_Type_NotKnown)]
        [InlineData(TestSamples.Section_Invalid_Property_NotKnown)]
        [InlineData(TestSamples.Section_Invalid_BadSyntax_MissingAt)]
        [InlineData(TestSamples.Section_Invalid_BadSyntax_MissingQuotes)]
        [InlineData(TestSamples.Section_Invalid_BadSyntax_MissingEqualSign)]
        [InlineData(TestSamples.Section_Invalid_BadSyntax_ColonInsteadEqualSign)]
        #endregion
        internal void BuildFromNavigationConext_IdentifySingleSection_SectionWithBadSyntax_Test(string sectionWithBadSyntax)
        {
            // Arrange
            var action = () => ExtendedFilteringProperties.BuildFromNavigationContext(sectionWithBadSyntax);
            // Act & assert
            Assert.Throws<ArgumentException>(action);
        }

        // TODO: add tests for the server section type

        private string GenerateSection(ContextLevel sectionType, string name, string? schema = null)
        {
            if(sectionType != ContextLevel.Table && schema is not null)
                throw new ArgumentException($"Schema is not applicable for context level '{sectionType}'.", nameof(schema));

            if (sectionType == ContextLevel.Table && schema is null)
                throw new ArgumentException($"Schema must be provided for context level '{sectionType}'.", nameof(schema));

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
