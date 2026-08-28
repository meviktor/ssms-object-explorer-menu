using System;
using System.Collections.Generic;

namespace SSMSObjectExplorerMenu
{
    public static class Utils
    {
        public static IEnumerable<string> ParametersFromContext =
            [   
                "OBJECT",
                "SERVER",
                "DATABASE",
                "TABLE",
                "VIEW",
                "STORED_PROCEDURE",
                "FUNCTION",
                "SCHEMA",
                "JOB",
                "YYYY-MM-DD",
                "HHmm:ss",
                "YYYY-MM-DD HH:mm:ss"
            ];

        public static T EnumParse<T>(string value) where T : Enum
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
