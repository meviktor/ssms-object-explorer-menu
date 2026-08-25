using SSMSObjectExplorerMenu.objects;
using SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes;
using System;
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
        private const byte SQL_SERVER_IDENTIFIER_MAX_LENGTH = 128;

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

        internal bool IsEmpty => Context is null;

        /// <summary>
        /// Returns the filter in string representation, resembling the navigation context format.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => !IsEmpty ? 
            $"{Server}{(Database != null ? $"/{Database}" : null)}{(Table != null ? $"/{Table}" : null)}{(Column != null ? $"/{Column}" : null)}" : string.Empty;

        internal static (bool IsValid, string Error) ValidateForContext(string menuItemContext, string filter)
        {
            if (menuItemContext == null)
                throw new ArgumentNullException(nameof(menuItemContext), $"Parameter '{nameof(menuItemContext)}' cannot be null.");

            // proceeding only for contexts: Server/Database/Table/Column
            if (!ExtendedFiltering_AllowedContexts.Contains(menuItemContext) && !string.IsNullOrEmpty(filter))
                return (false, $"Providing additional filter for context '{menuItemContext}' is not allowed. Filter must be left empty.");

            var menuItemContextLevel = menuItemContext.FromStringDescription<ContextLevel>();

            ExtendedFilteringProperties props;
            try
            {
                props = BuildFromNavigationContext(filter, menuItemContextLevel);
            }
            catch (ArgumentException ex)
            {
                return (false, $"Invalid additional filter: {ex.Message}");
            }

            if (!props.IsEmpty)
            {
                var filterContextLevel = props.Context.Value;
                if (filterContextLevel < menuItemContextLevel)
                {
                    // e.g. filter contains condition for columns, but the menu item meant to target tables instead
                    return (false, $"Additional filter targets '{filterContextLevel}' level, while it should target '{menuItemContextLevel}' or a higher level.");
                }
            }

            return (true, null);
        }

        /// <summary>
        /// Builds an <see cref="ExtendedFilteringProperties"/> instance from a navigation context string.
        /// </summary>
        /// <param name="navigationContext">The navigaton context belongs to the SSMS Object Explorer tree node.</param>
        /// <param name="buildingForLevel">If we are creating/updating a filter for a menu item, the menu item's context - enables validating if the filter string is
        /// appropriate for the menu item's context. Don't fill it otherwise.</param>
        /// <returns></returns>
        internal static ExtendedFilteringProperties BuildFromNavigationContext(string navigationContext, ContextLevel? buildingForLevel = null)
        {
            var navContextSections = navigationContext.Split(['/'], StringSplitOptions.RemoveEmptyEntries);

            var parsedSections = navContextSections.Select(IdentifySection).ToArray();
            if (parsedSections.Any(s => !s.IsIdentified))
            {
                throw new ArgumentException(
                    $"'{navigationContext}' contains either not allowed section types or sections with bad syntax. " + 
                    $"Accepted section types: {nameof(Server)}, {nameof(Database)}, {nameof(Table)}, {nameof(Column)}.");
            }

            var duplicatedSectionKinds = parsedSections.GroupBy(s => s.Kind).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
            if (duplicatedSectionKinds.Any())
            {
                throw new ArgumentException(
                    $"'{navigationContext}' contains duplicated sections: {string.Join(", ", duplicatedSectionKinds)}.");
            }

            // Ordering: for each section, take all preceding section, and check if any of them has a lower context level - it's a fail.
            // Correct order of sections: Server, Database, Table, Column.
            if (parsedSections.Select((section, index) => new { section, index })
                              .Any(e => parsedSections.Take(e.index).Any(prevSection => Utils.EnumParse<ContextLevel>(prevSection.Kind) < Utils.EnumParse<ContextLevel>(e.section.Kind))))
            {
                throw new ArgumentException(
                    $"'{navigationContext}' has invalid ordering. Correct order based on hierarchy: {nameof(Server)}, {nameof(Database)}, {nameof(Table)}, {nameof(Column)}.");
            }

            var columnSection = parsedSections.SingleOrDefault(s => s.Kind == nameof(Column));
            var tableSection = parsedSections.SingleOrDefault(s => s.Kind == nameof(Table));
            var databaseSection = parsedSections.SingleOrDefault(s => s.Kind == nameof(Database));
            var serverSection = parsedSections.SingleOrDefault(s => s.Kind == nameof(Server));

            var filter = new ExtendedFilteringProperties();
            filter.Column = columnSection != default ? new Column(columnSection.Properties.Single().Value) : null;
            filter.Table = tableSection != default ? new Table(
                tableSection.Properties.Single(p => p.Name == nameof(PropertyTypes.Table.Name)).Value,
                tableSection.Properties.Single(p => p.Name == nameof(PropertyTypes.Table.Schema)).Value)
                    : (filter.Column != null ? Table.Any : null);
            filter.Database = databaseSection != default ? new Database(databaseSection.Properties.Single().Value) 
                    : (filter.Table != null ? Database.Any : null);
            filter.Server = serverSection != default ? new Server(serverSection.Properties.Single().Value)
                    : (filter.Database != null ? Server.Any : null);

            var lengthErrorTemplate = $"The provided {{0}} '{{1}}' exceeds maximum length of {SQL_SERVER_IDENTIFIER_MAX_LENGTH} characters.";
            if (filter.Column?.Name.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                throw new ArgumentException(string.Format(lengthErrorTemplate, "column identifier", filter.Column.Name));
            if (filter.Table?.Name.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                throw new ArgumentException(string.Format(lengthErrorTemplate, "table identifier", filter.Table.Name));
            if (filter.Table?.Schema.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                throw new ArgumentException(string.Format(lengthErrorTemplate, "schema identifier", filter.Table.Schema));
            if (filter.Database?.Name.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                throw new ArgumentException(string.Format(lengthErrorTemplate, "database identifier", filter.Database.Name));
            if (filter.Server?.Name.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                throw new ArgumentException(string.Format(lengthErrorTemplate, "server identifier", filter.Server.Name));

            // Validate for menu item context - navigationContext cannot contain "lower level" segments, even if they are not filtering (using the '*' wildcard)
            // For example: if the menu item's context is 'Table', you can't provide a segment with 'Column' type in the filter, like: Column[@Name='*'] or Column[@Name='Id']
            if (buildingForLevel.HasValue &&
               ((buildingForLevel == ContextLevel.Server && (filter.Database != null || filter.Table != null || filter.Column != null)) ||
               (buildingForLevel == ContextLevel.Database && (filter.Table != null || filter.Column != null)) ||
               (buildingForLevel == ContextLevel.Table && filter.Column != null)))
                throw new ArgumentException(string.Format($"Additional filter targets '{buildingForLevel}' level. It cannot contain wildcard segment(s) for lower level(s)."));

            return filter;
        }

        private static NavContextSection IdentifySection(string section)
        {
            var serverMatch = Server.Regex.Match(section);
            if (serverMatch.Success)
            {
                return new () { Kind = nameof(Server), Properties = [(nameof(PropertyTypes.Server.Name), serverMatch.Groups[1].Value)] };
            }

            var databaseMatch = Database.Regex.Match(section);
            if (databaseMatch.Success)
            {
                return new() { Kind = nameof(Database), Properties = [(nameof(PropertyTypes.Database.Name), databaseMatch.Groups[1].Value)] };
            }

            var tableMatch = Table.Regex.Match(section);
            if (tableMatch.Success)
            {
                return new() { Kind = nameof(Table), Properties = [(nameof(PropertyTypes.Table.Name), tableMatch.Groups[1].Value), (nameof(PropertyTypes.Table.Schema), tableMatch.Groups[2].Value)] };
            }

            var columnMatch = Column.Regex.Match(section);
            if (columnMatch.Success)
            {
                return new() { Kind = nameof(Column), Properties = [(nameof(PropertyTypes.Column.Name), columnMatch.Groups[1].Value)] };
            }

            return new() { Kind = null, Properties = null };
        }

        class NavContextSection()
        {
            internal bool IsIdentified => !string.IsNullOrEmpty(Kind);

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
                throw new ArgumentException(nameof(node), $"Parameter '{nameof(node)}' does not filter for anything.");

            if (filter.IsNullOrEmpty())
                return true;

            // Our filter contains constraints for "lower level items", not applicable on the node. Example: filtering for table name on a 'Server' node.
            if (filter.Context < node.Context)
                return false;

            // Failing on column name: node is column, filter applies on columns; column name does not match.
            if (node.Context == ContextLevel.Column && filter.Column.IsActiveFilter && node.Column.Name != filter.Column.Name)
                return false;

            // Failing on table name and/or schema: node is column or table, filter applies on tables; table name/schema does not match.
            if (node.Context <= ContextLevel.Table && filter.Table.IsActiveFilter)
            {
                var tableOrSchemaNotMatch =
                    // Filtering for table name & schema: if any of them does not match, it's a fail
                    (filter.Table?.Name != Wildcard_Any && filter.Table?.Schema != Wildcard_Any && (node.Table?.Name != filter.Table?.Name || node.Table?.Schema != filter.Table?.Schema))
                    // Filtering for table name only: if table name does not match, it's a fail
                    || (filter.Table?.Name != Wildcard_Any && filter.Table?.Schema == Wildcard_Any && node.Table?.Name != filter.Table?.Name)
                    // Filtering for schema only: if schema does not match, it's a fail
                    || (filter.Table?.Name == Wildcard_Any && filter.Table?.Schema != Wildcard_Any && node.Table?.Schema != filter.Table?.Schema);

                if (tableOrSchemaNotMatch)
                    return false;
            }

            // Failing on database name: node is column, table or database, filter applies on databases; database name does not match.
            if (node.Context <= ContextLevel.Database && filter.Database.IsActiveFilter && node.Database?.Name != filter.Database?.Name)
                return false;

            // Failing on server name: any kind of node, filter applies on servers; server name does not match.
            if (filter.Server.IsActiveFilter && node.Server?.Name != filter.Server?.Name)
                return false;

            // Node passed filtering
            return true;
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
