using SSMSObjectExplorerMenu.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace SSMSObjectExplorerMenu.extensions
{
    internal static class StringExtensions
    {
        public static string ReplaceRange(this string original, IEnumerable<(string Phrase, string Value)> elements)
        {
            string result = original;
            foreach ((string Phrase, string Value) in elements)
            {
                var replacementRegex = Regex.Escape(Phrase);
                result = Regex.Replace(result, replacementRegex, Value, RegexOptions.IgnoreCase);
            }
            return result;
        }

        public static T? FromStringDescription<T>(this string description) where T : struct, Enum
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));

            return typeof(T).GetFields()
                    .Where(e => Attribute.GetCustomAttribute(e, typeof(DescriptionAttribute)) is DescriptionAttribute attr && attr.Description == description)
                    .Select(e => (T?)Enum.Parse(typeof(T), e.Name))
                    .SingleOrDefault();
        }

        public static bool ValidForUserDefinedParameterType(this string value, UserDefinedParameterType type)
        {
            return type switch
            {
                UserDefinedParameterType.UniqueIdentifier => Guid.TryParse(value, out _),
                UserDefinedParameterType.Nvarchar => true,// Any string is valid for nvarchar
                UserDefinedParameterType.DateTime2 => DateTime.TryParse(value, out _),
                UserDefinedParameterType.DateTimeOffset => DateTimeOffset.TryParse(value, out _),
                UserDefinedParameterType.Int => int.TryParse(value, out _),
                UserDefinedParameterType.Bit => value == "0" || value == "1",
                UserDefinedParameterType.CustomList => throw new ArgumentException($"Operation is not applicable for type '{type}'."),
                _ => throw new NotImplementedException($"Validation for parameter type {type} has not been implemented."),
            };
        }
    }
}
