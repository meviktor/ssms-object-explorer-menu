using SSMSObjectExplorerMenu.advancedfiltering.exceptions;
using SSMSObjectExplorerMenu.advancedfiltering.enums;
using SSMSObjectExplorerMenu.advancedfiltering.models;
using SSMSObjectExplorerMenu.enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSMSObjectExplorerMenu.advancedfiltering
{
    internal class DatabaseObjectFilter
    {
        internal MenuItemContext Context { get; private set; }

        internal ServerProperties Server { get; private set; } = ServerProperties.Empty;

        internal FolderProperties Folder { get; private set; } = FolderProperties.Empty;

        internal DatabaseProperties Database { get; private set; } = DatabaseProperties.Empty;

        internal TableProperties Table { get; private set; } = TableProperties.Empty;

        internal ColumnProperties Column { get; private set; } = ColumnProperties.Empty;

        internal ViewProperties View { get; private set; } = ViewProperties.Empty;

        internal StoredProcedureProperties StoredProcedure { get; private set; } = StoredProcedureProperties.Empty;

        internal JobServerProperties JobServer { get; private set; } = JobServerProperties.Empty;

        internal JobProperties Job { get; private set; } = JobProperties.Empty;

        protected DatabaseObjectFilter() { }

        internal bool MatchesWithRule(DatabaseObjectFilter rule)
        {
            return Context == rule.Context &&
                Server.MatchesWithRule(rule.Server) &&
                Folder.MatchesWithRule(rule.Folder) &&
                Database.MatchesWithRule(rule.Database) &&
                Table.MatchesWithRule(rule.Table) &&
                Column.MatchesWithRule(rule.Column) &&
                View.MatchesWithRule(rule.View) &&
                StoredProcedure.MatchesWithRule(rule.StoredProcedure) &&
                JobServer.MatchesWithRule(rule.JobServer) &&
                Job.MatchesWithRule(rule.Job);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;

            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (DatabaseObjectFilter)obj;
            return Context == other.Context &&
                Equals(Server, other.Server) &&
                Equals(Folder, other.Folder) &&
                Equals(Database, other.Database) &&
                Equals(Table, other.Table) &&
                Equals(Column, other.Column) &&
                Equals(View, other.View) &&
                Equals(StoredProcedure, other.StoredProcedure) &&
                Equals(JobServer, other.JobServer) &&
                Equals(Job, other.Job);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Context.GetHashCode();
                hash = hash * 23 + Server.GetHashCode();
                hash = hash * 23 + Folder.GetHashCode();
                hash = hash * 23 + Database.GetHashCode();
                hash = hash * 23 + Table.GetHashCode();
                hash = hash * 23 + Column.GetHashCode();
                hash = hash * 23 + View.GetHashCode();
                hash = hash * 23 + StoredProcedure.GetHashCode();
                hash = hash * 23 + JobServer.GetHashCode();
                hash = hash * 23 + Job.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(DatabaseObjectFilter left, DatabaseObjectFilter right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(DatabaseObjectFilter left, DatabaseObjectFilter right)
        {
            return !(left == right);
        }

        internal static DatabaseObjectFilter Build(MenuItemContext context, string filter)
        {
            if (context == MenuItemContext.All)
            {
                throw new ArgumentException($"Parameter '{nameof(context)}' cannot be '{MenuItemContext.All}'.", nameof(context));
            }

            if (string.IsNullOrWhiteSpace(filter))
            {
                throw new ArgumentException($"Parameter '{nameof(filter)}' cannot be null or whitespace.", nameof(filter));
            }

            var filterObj = new DatabaseObjectFilter();
            filterObj.Context = context;

            var sections = filter?.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var lastSectionType = FilterSection.None;

            try
            {
                foreach (var section in sections)
                {
                    lastSectionType = ParseAndSet(section, lastSectionType, filterObj);
                }
            }
            catch(Exception e)
            {
                throw new ArgumentException($"Cannot use filter string to build the filter object.", nameof(filter), e);
            }

            return filterObj;
        }

        private static FilterSection ParseAndSet(string filterSection, FilterSection lastSectionType, DatabaseObjectFilter filter)
        {
            FilterSection parsedSectionType;

            // Parse a properties object from the incoming section
            var propertiesSectionStartIndex = filterSection.IndexOf('[');
            // TODO: do we have to do anything extra if the section does not have any properties, so [ ( and ] ) characters? Like in case of 'JobServer' section...
            var currentSectionType = filterSection.Substring(0, propertiesSectionStartIndex);
            if(!_allowedSectionNamesLowerCase.Contains(currentSectionType.ToLower()))
            {
                throw new NotSupportedException($"Parsing of section type '{currentSectionType}' is not supported.");
            }

            // Decide wheter this section can follow the last one
            parsedSectionType = (FilterSection)Enum.Parse(typeof(FilterSection), currentSectionType, true);
            if (!_directDescandants[lastSectionType].Contains(parsedSectionType))
            {
                var possibleOptions = string.Join("', '", _directDescandants[lastSectionType]);
                throw new FormatException($"A section of type '{lastSectionType}' cannot be followed with a section of type '{parsedSectionType}'. Possible options: {possibleOptions}");
            }

            // Validating the "outer" structure of the properties part [ @PropertyName='PropertyValue' (and ...) ]
            var propertiesSectionEndIndex = filterSection.IndexOf("]");
            var propertySections = filterSection
                // taking the properties part without the opening/closing characters ( [, ] )
                .Substring(propertiesSectionStartIndex + 1, propertiesSectionEndIndex - 1)
                // NOT removing empty entries - those imply syntax error in the filter string (like: no condition given on the left/right side of the 'and' operator)
                .Split(new[] { "and" }, StringSplitOptions.None);

            // no condition given or using no condition given on the left/right side of 'and' operator
            if (propertySections.Length == 0 || propertySections.Any(section => string.IsNullOrEmpty(section.Trim())))
            {
                throw new FormatException($"Filter section '{filterSection}' has syntax error(s) - no conditions given or invalid usage of the 'and' operator.");
            }

            var propertiesDict = new Dictionary<string, string>();
            // Validating each property collection item (@PropertyName='PropertyValue') and adding it to the properties dictionary
            foreach (var propertySection in propertySections)
            {
                // NOT removing empty entries - those imply syntax error in the filter string (like: no property name or value given)
                var propertySectionElements = propertySection.Split(new[] { "=" }, StringSplitOptions.None);
                if(propertySectionElements.Length != 2 ||
                    string.IsNullOrEmpty(propertySectionElements[0].Trim()) || // property name
                    string.IsNullOrEmpty(propertySectionElements[1].Trim())) // property value
                {
                    throw new FormatException($"Filter section '{filterSection}' has syntax error(s) - each property must have a name and value.");
                }

                var propertyName = propertySectionElements[0].Trim();
                if(!propertyName.StartsWith("@") || propertyName.Length < 2)
                {
                    throw new FormatException($"Filter section '{filterSection}' has syntax error(s) - property name has to start with @ and must be at least one character long.");
                }

                var propertyValue = propertySectionElements[1].Trim();
                if (!propertyValue.StartsWith("'") || !propertyValue.EndsWith("'") || propertyName.Length < 3)
                {
                    throw new FormatException($"Filter section '{filterSection}' has syntax error(s) - property value must be put between quotes (') and must be at least one character long.");
                }

                propertiesDict.Add(
                    propertyName.Substring(1).ToLower(), // property name without the starting @
                    propertyValue.Substring(1, propertyValue.Length - 2)); // value name without quotes (')
            }

            // Buidling the filter section; checking for any missing- or unsupported properties
            try
            {
                AddSectionToFilter(parsedSectionType, propertiesDict, filter);
            }
            catch (MissingPropertyException me)
            {
                throw new FormatException(
                    $"Filter section '{filterSection}' has syntax error(s) - the following required properties are missing: {string.Join(", ", me.PropertyNames)}.", me);
            }
            catch (UnsupportedPropertyException ue)
            {
                throw new FormatException(
                    $"Filter section '{filterSection}' has syntax error(s) - the following properties are not supported: {string.Join(", ", ue.PropertyNames)}.", ue);
            }

            return parsedSectionType;
        }

        private static void AddSectionToFilter(FilterSection sectionType, Dictionary<string, string> propertiesDict, DatabaseObjectFilter filter)
        {
            switch (sectionType)
            {
                case FilterSection.Server:
                    var serverName = nameof(ServerProperties.Name).ToLower();
                    ThrowIfUnsupportedOrMissingProperties(sectionType, propertiesDict.Keys, serverName);
                    filter.Server = new ServerProperties(propertiesDict[serverName]);
                    break;
                case FilterSection.Folder:
                    var folderName = nameof(FolderProperties.Name).ToLower();
                    var folderType = nameof(FolderProperties.Type).ToLower();
                    ThrowIfUnsupportedOrMissingProperties(sectionType, propertiesDict.Keys, folderName, folderType);
                    filter.Folder = new FolderProperties(propertiesDict[folderName], propertiesDict[folderType]);
                    break;
                case FilterSection.Database:
                    var databaseName = nameof(DatabaseProperties.Name).ToLower();
                    ThrowIfUnsupportedOrMissingProperties(sectionType, propertiesDict.Keys, databaseName);
                    filter.Database = new DatabaseProperties(propertiesDict[databaseName]);
                    break;
                case FilterSection.Table:
                    var tableName = nameof(TableProperties.Name).ToLower();
                    var tableSchema = nameof(TableProperties.Schema).ToLower();
                    ThrowIfUnsupportedOrMissingProperties(sectionType, propertiesDict.Keys, tableName, tableSchema);
                    filter.Table = new TableProperties(propertiesDict[tableName], propertiesDict[tableSchema]);
                    break;
                case FilterSection.Column:
                    var columnName = nameof(ColumnProperties.Name).ToLower();
                    ThrowIfUnsupportedOrMissingProperties(sectionType, propertiesDict.Keys, columnName);
                    filter.Column = new ColumnProperties(propertiesDict[columnName]);
                    break;
                case FilterSection.StoredProcedure:
                    var spName = nameof(StoredProcedureProperties.Name).ToLower();
                    var spSchema = nameof(StoredProcedureProperties.Schema).ToLower();
                    ThrowIfUnsupportedOrMissingProperties(sectionType, propertiesDict.Keys, spName, spSchema);
                    filter.StoredProcedure = new StoredProcedureProperties(propertiesDict[spName], propertiesDict[spSchema]);
                    break;
                case FilterSection.View:
                    var viewName = nameof(ViewProperties.Name).ToLower();
                    var viewSchema = nameof(ViewProperties.Schema).ToLower();
                    ThrowIfUnsupportedOrMissingProperties(sectionType, propertiesDict.Keys, viewName, viewSchema);
                    filter.View = new ViewProperties(propertiesDict[viewName], propertiesDict[viewSchema]);
                    break;
                case FilterSection.JobServer:
                    filter.JobServer = new JobServerProperties(true);
                    break;
                case FilterSection.Job:
                    var jobName = nameof(JobProperties.Name).ToLower();
                    ThrowIfUnsupportedOrMissingProperties(sectionType, propertiesDict.Keys, jobName);
                    filter.Job = new JobProperties(propertiesDict[jobName]);
                    break;
                default:
                    throw new NotImplementedException($"Implementation missing for '{sectionType}'.");
            }
        }

        private static void ThrowIfUnsupportedOrMissingProperties(FilterSection sectionType, Dictionary<string, string>.KeyCollection keys, params string[] supportedPropertyNames)
        {
            var notSupportedProperties = keys.Where(key => !supportedPropertyNames.Contains(key));
            if (notSupportedProperties.Any())
            {
                throw new UnsupportedPropertyException(sectionType, notSupportedProperties);
            }

            var missingRequiredProperties = supportedPropertyNames.Where(prop => !keys.Contains(prop));
            if (missingRequiredProperties.Any())
            {
                throw new MissingPropertyException(sectionType, missingRequiredProperties);
            }
        }

        /// <summary>
        /// Describes what filter sections can follow directly a specific filter section.
        /// These rules were created according the filter strings of the <see cref="AdvancedFilters"/> class (<see cref="AdvancedFilters.Table"/>, etc.)
        /// </summary>
        private static Dictionary<FilterSection, FilterSection[]> _directDescandants = new Dictionary<FilterSection, FilterSection[]>
        {
            // Filter strings start with Server section
            { FilterSection.None, new FilterSection[] { FilterSection.Server } },
            { FilterSection.Server, new FilterSection[] { FilterSection.Folder, FilterSection.Database, FilterSection.JobServer } },
            { FilterSection.Folder, Array.Empty<FilterSection>() },
            { FilterSection.Database, new FilterSection[] { FilterSection.Folder, FilterSection.Table, FilterSection.View, FilterSection.StoredProcedure } },
            { FilterSection.Table, new FilterSection[] { FilterSection.Column } },
            { FilterSection.StoredProcedure, Array.Empty<FilterSection>() },
            { FilterSection.View, Array.Empty<FilterSection>() },
            { FilterSection.JobServer, new FilterSection[] { FilterSection.Folder, FilterSection.Job } },
            { FilterSection.Job, Array.Empty<FilterSection>() }
        };

        private static string[] _allowedSectionNamesLowerCase = Enum.GetNames(typeof(FilterSection)).Select(name => name.ToLower()).ToArray();
    }
}
