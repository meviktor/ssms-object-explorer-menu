using System;
using System.ComponentModel;

namespace SSMSObjectExplorerMenu.extensions
{
    internal static class EnumExtensions
    {
        public static string ToStringDescription<T>(this T context) where T : Enum
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, context);
            if (name != null)
            {
                var field = enumType.GetField(name);
                if (field != null)
                {
                    var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            throw new ArgumentException($"Unknown {nameof(T)} value.", nameof(context));
        }
    }
}
