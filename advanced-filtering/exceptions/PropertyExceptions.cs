using SSMSObjectExplorerMenu.advancedfiltering.enums;
using System;
using System.Collections.Generic;

namespace SSMSObjectExplorerMenu.advancedfiltering.exceptions
{
    internal abstract class PropertyException : Exception
    {
        internal FilterSection SectionType { get; private set; }

        internal IEnumerable<string> PropertyNames { get; private set; }

        internal PropertyException(FilterSection sectionType, IEnumerable<string> propertyNames) : base() 
        {
            SectionType = sectionType;
            PropertyNames = propertyNames;
        }

        internal PropertyException(FilterSection sectionType, IEnumerable<string> propertyNames, string message) : base(message)
        {
            SectionType = sectionType;
            PropertyNames = propertyNames;
        }
    }

    internal class UnsupportedPropertyException : PropertyException
    {
        public UnsupportedPropertyException(FilterSection sectionType, IEnumerable<string> propertyNames) : base(sectionType, propertyNames) { }

        public UnsupportedPropertyException(FilterSection sectionType, IEnumerable<string> propertyNames, string message) : base(sectionType, propertyNames, message) { }
    }

    internal class MissingPropertyException : PropertyException
    {
        public MissingPropertyException(FilterSection sectionType, IEnumerable<string> propertyNames) : base(sectionType, propertyNames) { }

        public MissingPropertyException(FilterSection sectionType, IEnumerable<string> propertyNames, string message) : base(sectionType, propertyNames, message) { }
    }
}
