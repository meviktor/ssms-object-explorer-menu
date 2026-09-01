using SSMSObjectExplorerMenu.objects;
using SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static SSMSObjectExplorerMenu.Constants;
using static SSMSObjectExplorerMenu.extendedfiltering.ExtendedFiltering;
using SSMSObjectExplorerMenu.extensions;

namespace SSMSObjectExplorerMenu.extendedfiltering
{
    /// <summary>
    /// Filter class for extended filtering feature.<br/>
    /// Its main attribute is the ContextLevel, which indicates the lowest context level where a condition is defined (Server, Database, Table, Column).<br/>
    /// Filter anatomy example - Table:<br/>
    /// - Server: Name='localhost' (although this filter targets table level, additional conditions may be defined on higher levels for more precise filtering)<br/>
    /// - Database: Database.Any (no condition defined)<br/>
    /// - Table: Schema='dbo', Name='Customers' (the first/lowest level where a condition is defined)<br/>
    /// - Column: null (not applicable)<br/>
    /// <br/>
    /// As a summary, filter components for:<br/>
    /// - Lower levels than the target: must be set to null.<br/>
    /// - Target level: a condition must be set.<br/>
    /// - Higher levels than the target: may have conditions set. If not, use 'Any' (no restriction) instead of null.
    /// </summary>
    internal class ExtendedFilteringProperties
    {
        internal Server Server { get; private set; }

        internal Database Database { get; private set; }

        internal Table Table { get; private set; }

        internal Column Column { get; private set; }

        protected ExtendedFilteringProperties() { }

        internal ExtendedFilteringProperties(Server server, Database database = null, Table table = null, Column column = null)
        {
            if (server is null)
                throw new ArgumentNullException(nameof(server), $"Parameter '{nameof(server)}' cannot be null.");

            Server = server;
            Database = database;
            Table = table;
            Column = column;
        }

        /// <summary>
        /// Returns the context level the filtering properties are targeting. This means the lowest applicable (not null) context level where a condition is defined.
        /// </summary>
        internal ContextLevel? Context
        {
            get
            {
                if (Column.IsActiveFilter) return ContextLevel.Column;
                if (Table.IsActiveFilter) return ContextLevel.Table;
                if (Database.IsActiveFilter) return ContextLevel.Database;
                if (Server.IsActiveFilter) return ContextLevel.Server;

                return null;
            }
        }

        internal ContextLevel[] FilterSections
        {
            get
            {
                var _sections = new List<ContextLevel>();

                if (Column != null) _sections.Add(ContextLevel.Column);
                if (Table != null) _sections.Add(ContextLevel.Table);
                if (Database != null) _sections.Add(ContextLevel.Database);
                if (Server != null) _sections.Add(ContextLevel.Server);

                return [.. _sections];
            }
        }

        internal bool IsEmpty => Context is null;

        /// <summary>
        /// Returns the filter in string representation, resembling the navigation context format.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Server}{(Database != null ? $"/{Database}" : null)}{(Table != null ? $"/{Table}" : null)}{(Column != null ? $"/{Column}" : null)}";

        /// <summary>
        /// Builds an <see cref="ExtendedFilteringProperties"/> instance from a navigation context string.
        /// </summary>
        /// <param name="navigationContext">The navigaton context belongs to the SSMS Object Explorer tree node.</param>
        /// <param name="useRegularIdentifiers">TODO!</param>
        /// <returns></returns>
        internal static ExtendedFilteringProperties BuildFromNavigationContext(string navigationContext, bool useRegularIdentifiers, out IEnumerable<string> errors)
        {
            var navContextSections = navigationContext?
                .Split(['/'], StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s)) ?? [];

            var parsedSections = navContextSections.Select(s => IdentifySection(s, useRegularIdentifiers)).ToArray();
            if (parsedSections.Any(s => !s.IsIdentified))
            {
                // In case the string contains unidentifiable section(s): stop collecting further errors
                errors = [ ERROR_BUILD_FILTER_UNKNOWN_SECTION ];
                return null;
            }

            errors = [];

            var duplicatedSectionKinds = parsedSections.GroupBy(s => s.Kind).Any(g => g.Count() > 1);
            if (duplicatedSectionKinds)
                errors = errors.Append(ERROR_BUILD_FILTER_DUPLICATED_SECTION);

            // Ordering: for each section, take all preceding section, and check if any of them has a lower context level - it's a fail.
            // Correct order of sections: Server, Database, Table, Column.
            if (parsedSections.Select((section, index) => new { section, index })
                              .Any(e => parsedSections.Take(e.index).Any(prevSection => Utils.EnumParse<ContextLevel>(prevSection.Kind) < Utils.EnumParse<ContextLevel>(e.section.Kind))))
                errors = errors.Append(ERROR_BUILD_FILTER_INVALID_SECTION_ORDER);

            var columnSection = parsedSections.FirstOrDefault(s => s.Kind == nameof(Column));
            var tableSection = parsedSections.FirstOrDefault(s => s.Kind == nameof(Table));
            var databaseSection = parsedSections.FirstOrDefault(s => s.Kind == nameof(Database));
            var serverSection = parsedSections.FirstOrDefault(s => s.Kind == nameof(Server));

            if (columnSection?.HasValueSyntaxErrors == true)
                errors = errors.Append(useRegularIdentifiers ? ERROR_BUILD_FILTER_COLUMN_NAME_NOT_A_REGULAR_IDENTIFIER : ERROR_BUILD_FILTER_COLUMN_NAME_UNESCAPED_QUOTES);

            if (tableSection?.HasValueSyntaxErrors == true)
                errors = errors.Append(useRegularIdentifiers ? ERROR_BUILD_FILTER_TABLE_NAME_NOT_A_REGULAR_IDENTIFIER : ERROR_BUILD_FILTER_TABLE_NAME_UNESCAPED_QUOTES);

            if (databaseSection?.HasValueSyntaxErrors == true)
                errors = errors.Append(useRegularIdentifiers ? ERROR_BUILD_FILTER_DATABASE_NAME_NOT_A_REGULAR_IDENTIFIER : ERROR_BUILD_FILTER_DATABASE_NAME_UNESCAPED_QUOTES);

            if (serverSection?.HasValueSyntaxErrors == true)
                errors = errors.Append(ERROR_BUILD_FILTER_SERVER_NAME_UNESCAPED_QUOTES);

            var columnName = columnSection?.Properties.Single().Value;
            var tableName = tableSection?.Properties.Single(p => p.Name == nameof(PropertyTypes.Table.Name)).Value;
            var schemaName = tableSection?.Properties.Single(p => p.Name == nameof(PropertyTypes.Table.Schema)).Value;
            var databaseName = databaseSection?.Properties.Single().Value;
            var serverName = serverSection?.Properties.Single().Value;

            if (columnName?.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                errors = errors.Append(ERROR_BUILD_FILTER_COLUMN_NAME_TOO_LONG);

            if (tableName?.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                errors = errors.Append(ERROR_BUILD_FILTER_TABLE_NAME_TOO_LONG);

            if (schemaName?.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                errors = errors.Append(ERROR_BUILD_FILTER_SCHEMA_NAME_TOO_LONG);

            if (databaseName?.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                errors = errors.Append(ERROR_BUILD_FILTER_DATABASE_NAME_TOO_LONG);

            if (serverName?.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                errors = errors.Append(ERROR_BUILD_FILTER_SERVER_NAME_TOO_LONG);

            if (errors.Any())
                return null;

            var filter = new ExtendedFilteringProperties();
            filter.Column = columnSection != null ? new Column(columnName) : null;
            filter.Table = tableSection != null ? new Table(tableName, schemaName) : (filter.Column != null ? Table.Any : null);
            filter.Database = databaseSection != null ? new Database(databaseName) : (filter.Table != null ? Database.Any : null);
            filter.Server = serverSection != null ? new Server(serverName) : (filter.Database != null ? Server.Any : null);

            return filter;
        }

        private static NavContextSection IdentifySection(string section, bool useRegularIdentifiers)
        {
            static NavContextSection toNavContextSection(ValidationResult result, string kind) 
                => new() { Kind = kind, HasValueSyntaxErrors = result.HasSyntaxErrors, Properties = result.ExtractedProperties };

            ValidationResult validationResult;
            
            validationResult = Server.Validate(section);
            if (validationResult.IsIdentified)
                return toNavContextSection(validationResult, nameof(Server));

            validationResult = Database.Validate(section, useRegularIdentifiers);
            if (validationResult.IsIdentified)
                return toNavContextSection(validationResult, nameof(Database));

            validationResult = Table.Validate(section, useRegularIdentifiers);
            if (validationResult.IsIdentified)
                return toNavContextSection(validationResult, nameof(Table));

            validationResult = Column.Validate(section, useRegularIdentifiers);
            if (validationResult.IsIdentified)
                return toNavContextSection(validationResult, nameof(Column));

            return new() { Kind = null, HasValueSyntaxErrors = null, Properties = [] };
        }

        class NavContextSection()
        {
            internal bool IsIdentified => !string.IsNullOrEmpty(Kind);

            internal bool? HasValueSyntaxErrors { get; set; }

            internal string Kind { get; set; }

            internal (string Name, string Value)[] Properties { get; set; }
        }
    }

    internal static class ExtendedFilteringPropertiesExtensions
    {
        internal static bool IsNullOrEmpty(this ExtendedFilteringProperties props) => props is null || props.IsEmpty;

        /// <summary>
        /// Applies the filter of a <see cref="MenuItem"/> on an SSMS Object Explorer node.
        /// </summary>
        /// <param name="node">The <see cref="ExtendedFilteringProperties"/> representing the node.</param>
        /// <param name="filter">The <see cref="ExtendedFilteringProperties"/> representing the filter.</param>
        /// <returns>True if the node passes the filter's criteria, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If node is null.</exception>
        internal static bool ApplyFiltering(this ExtendedFilteringProperties node, ExtendedFilteringProperties filter)
        {
            if (node is null)
                throw new ArgumentNullException(nameof(node), $"Parameter '{nameof(node)}' cannot be null.");
            if (node.IsEmpty)
                throw new ArgumentException(nameof(node), $"Parameter '{nameof(node)}' does not designate an SSMS Object Explorer node.");

            if (filter.IsNullOrEmpty())
                return true;

            // Our filter contains constraints for "lower level items", not applicable on the node. Example: filtering for table name on a 'Server' node.
            if (filter.Context < node.Context)
                return false;

            // Failing on column name: node is column, filter applies on columns; column name does not match.
            if (node.Context == ContextLevel.Column && filter.Column.IsActiveFilter && node.Column != filter.Column)
                return false;

            // Failing on table name and/or schema: node is column or table, filter applies on tables; table name/schema does not match.
            if (node.Context <= ContextLevel.Table && filter.Table.IsActiveFilter)
            {
                var tableNamesMatch = node.Table?.Name.Equals(filter.Table?.Name, StringComparison.OrdinalIgnoreCase) ?? false;
                var schemaNamesMatch = node.Table?.Schema.Equals(filter.Table?.Schema, StringComparison.OrdinalIgnoreCase) ?? false;
                var tableOrSchemaNotMatch =
                    // Filtering for table name & schema: if any of them does not match, it's a fail
                    (filter.Table?.Name != Wildcard_Any && filter.Table?.Schema != Wildcard_Any && (!tableNamesMatch || !schemaNamesMatch))
                    // Filtering for table name only: if table name does not match, it's a fail
                    || (filter.Table?.Name != Wildcard_Any && filter.Table?.Schema == Wildcard_Any && !tableNamesMatch)
                    // Filtering for schema only: if schema does not match, it's a fail
                    || (filter.Table?.Name == Wildcard_Any && filter.Table?.Schema != Wildcard_Any && !schemaNamesMatch);

                if (tableOrSchemaNotMatch)
                    return false;
            }

            // Failing on database name: node is column, table or database, filter applies on databases; database name does not match.
            if (node.Context <= ContextLevel.Database && filter.Database.IsActiveFilter && node.Database != filter.Database)
                return false;

            // Failing on server name: any kind of node, filter applies on servers; server name does not match.
            if (filter.Server.IsActiveFilter && node.Server != filter.Server)
                return false;

            // Node passed filtering
            return true;
        }

        internal static bool TryValidateForContext(this ExtendedFilteringProperties filter, string targetContext, out IEnumerable<string> errors)
        {
            errors = [];

            // TODO: align tests
            if (filter == null)
                errors = errors.Append(ERROR_FILTER_VALIDATE_CONTEXT_NO_FILTER);

            var targetContextProvided = !string.IsNullOrEmpty(targetContext);
            if (!targetContextProvided)
                errors = errors.Append(ERROR_FILTER_VALIDATE_CONTEXT_NO_CONTEXT);

            var targetContextApplicable = ExtendedFiltering_AllowedContexts.ContainsKey(targetContext);
            if (!targetContextApplicable)
                errors = errors.Append(ERROR_FILTER_VALIDATE_CONTEXT_CONTEXT_NOT_APPLICABLE);

            if (filter != null && targetContextApplicable)
            {
                var targetCtxLvl = ExtendedFiltering_AllowedContexts[targetContext];
                // E.g. filter has e.g. a Column[Name="*"] segment, while the menu item applies for tables...
                // TODO: write & align tests
                if (filter.FilterSections.Any(s => s < targetCtxLvl))
                    errors = errors.Append(ERROR_FILTER_VALIDATE_LOW_CONTEXT_SEGMENT);

                var targetContextLevel = targetContext.FromStringDescription<ContextLevel>();
                if (filter.Context < targetContextLevel)
                    errors = errors.Append(ERROR_FILTER_VALIDATE_CONTEXT_FILTER_TARGETS_LOW_CONTEXT);
            }

            return !errors.Any();
        }
    }

    internal enum ContextLevel : byte
    {
        [Description(Column_Context)]
        Column = 0,
        [Description(Table_Context)]
        Table = 1,
        [Description(Database_Context)]
        Database = 2,
        [Description(Server_Context)]
        Server = 3
    }
}
