using SSMSObjectExplorerMenu.objects;
using SSMSObjectExplorerMenu.extendedfiltering.PropertyTypes;
using System;
using System.ComponentModel;
using System.Linq;

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
        private static byte SQL_SERVER_IDENTIFIER_MAX_LENGTH = 128;

        internal Server Server { get; private set; }

        internal Database Database { get; private set; }

        internal Table Table { get; private set; }

        internal Column Column { get; private set; }

        protected ExtendedFilteringProperties() { }

        internal ExtendedFilteringProperties(Server server, Database database = null, Table table = null, Column column = null)
        {
            if(server is null)
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
                if (Column != null && Column != Column.Any) return ContextLevel.Column;
                if (Table != null && Table != Table.Any) return ContextLevel.Table;
                if (Database != null && Database != Database.Any) return ContextLevel.Database;
                if (Server != null && Server != Server.Any) return ContextLevel.Server;

                return null;
            }
        }

        internal bool IsValid => Context != null;

        /// <summary>
        /// Returns the filter in string representation, resembling the navigation context format.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Server}{(Database != null ? $"/{Database}" : null)}{(Table != null ? $"/{Table}" : null)}{(Column != null ? $"/{Column}" : null)}";

        internal static (bool IsValid, string Error) ValidateForContext(string context, string filter)
        {
            if(context == null)
                throw new ArgumentNullException(nameof(context), $"Parameter '{nameof(context)}' cannot be null.");

            var allowedContextTypes = new string[] { Constants.Server_Context, Constants.Database_Context, Constants.Table_Context, Constants.Column_Context };

            if (!allowedContextTypes.Contains(context) && !string.IsNullOrEmpty(filter))
                return (false, $"Providing additional filter for context {context} is not allowed. Filter must be left empty.");

            ExtendedFilteringProperties props;
            try
            {
                props = BuildFromNavigationContext(filter);
            }
            catch (ArgumentException ex)
            {
                return (false, $"Invalid additional filter: {ex.Message}");
            }

            if(props == null)
                return (false, $"Additional filter does not contain any condition.");

            var filterContext = props.Context.Value;
            if((filterContext == ContextLevel.Server && context != Constants.Server_Context) ||
               (filterContext == ContextLevel.Database && context != Constants.Database_Context) ||
               (filterContext == ContextLevel.Table && context != Constants.Table_Context) ||
               (filterContext == ContextLevel.Column && context != Constants.Column_Context))
            {
                return (false, $"Filter is targeting {filterContext} level, while it should target {context} level.");
            }

            return (true, null);
        }

        /// <summary>
        /// Checks if the filtering properties of an SSMS Object Explorer node comply to the filtering properties of a <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="subject">The filtering properites built from the navigation context of a node in the SSMS Object Explorer tree.</param>
        /// <param name="expectation">The filtering properties of a <see cref="MenuItem"/> (custom command).</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If source or target is null.</exception>
        internal static bool ComplyTo(ExtendedFilteringProperties subject, ExtendedFilteringProperties expectation)
        {
            if(subject is null)
                throw new ArgumentNullException(nameof(subject), $"Parameter '{nameof(subject)}' cannot be null.");
            if(expectation is null)
                throw new ArgumentNullException(nameof(expectation), $"Parameter '{nameof(expectation)}' cannot be null.");
            if (!subject.IsValid)
                throw new ArgumentException(nameof(subject), $"Parameter '{nameof(subject)}' does not filter for anything.");
            if (!expectation.IsValid)
                throw new ArgumentException(nameof(expectation), $"Parameter '{nameof(expectation)}' does not filter for anything.");

            if (subject.Context != expectation.Context)
                return false;

            var contextLevel = subject.Context;

            // Column does not match while targeting column level.
            if (contextLevel == ContextLevel.Column && subject.Column.Name != expectation.Column.Name)
                return false;
            // Table does not match. It's only relevant, if the MenuItem's filter:
            // - Targets the table level, or
            // - Targets a column, but also filters for table name/schema
            if ((subject.Table?.Name != expectation.Table?.Name || subject.Table?.Schema != expectation.Table?.Schema) &&
               (contextLevel == ContextLevel.Table || (contextLevel < ContextLevel.Table && expectation.Table != Table.Any)))
                return false;
            // Database does not match. It's only relevant, if the MenuItem's filter:
            // - Targets the database level, or
            // - Targets a column or a table, but also filters for database name
            if ((subject.Database?.Name != expectation.Database?.Name) &&
                (contextLevel == ContextLevel.Database || (contextLevel < ContextLevel.Database && expectation.Database != Database.Any)))
                return false;
            // Server does not match. It's only relevant, if the MenuItem's filter:
            // - Targets the server level, or
            // - Targets a column, a table, or a database, but also filters for server name
            if ((subject.Server?.Name != expectation.Server?.Name) &&
                (contextLevel == ContextLevel.Server || (contextLevel < ContextLevel.Server && expectation.Server != Server.Any)))
                return false;

            // All checks passed. Object Explorer node complies to MenuItem filtering properties.
            return true;
        }

        /// <summary>
        /// Builds an <see cref="ExtendedFilteringProperties"/> instance from a navigation context string.
        /// </summary>
        /// <param name="navigationContext">The navigaton context belongs to the SSMS Object Explorer tree node.</param>
        /// <returns></returns>
        internal static ExtendedFilteringProperties BuildFromNavigationContext(string navigationContext)
        {
            var navContextSections = navigationContext.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var parsedSections = navContextSections.Select(s => IdentifySection(s)).ToArray();
            if (parsedSections.Any(s => !s.IsIdentified))
            {
                throw new ArgumentException(
                    $"'{navigationContext}' contains either not allowed section types or sections with bad syntax. " + 
                    $"Accepted section types: {nameof(Server)}, {nameof(Database)}, {nameof(Table)}, {nameof(Column)}.");
            }

            var duplicatedSectionKinds = parsedSections.GroupBy(s => s.Kind).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
            if(duplicatedSectionKinds.Any())
            {
                throw new ArgumentException(
                    $"'{navigationContext}' contains duplicated sections: {string.Join(", ", duplicatedSectionKinds)}.");
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
            if(filter.Table?.Name.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                throw new ArgumentException(string.Format(lengthErrorTemplate, "table identifier", filter.Table.Name));
            if (filter.Table?.Schema.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                throw new ArgumentException(string.Format(lengthErrorTemplate, "schema identifier", filter.Table.Schema));
            if (filter.Database?.Name.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                throw new ArgumentException(string.Format(lengthErrorTemplate, "database identifier", filter.Database.Name));
            if(filter.Server?.Name.Length > SQL_SERVER_IDENTIFIER_MAX_LENGTH)
                throw new ArgumentException(string.Format(lengthErrorTemplate, "server identifier", filter.Server.Name));

            return filter.IsValid ? filter : null;
        }

        private static (bool IsIdentified, string Kind, (string Name, string Value)[] Properties) IdentifySection(string section)
        {
            var serverMatch = Server.Regex.Match(section);
            if (serverMatch.Success)
            {
                return (true, nameof(Server), new (string, string)[] { (nameof(PropertyTypes.Server.Name), serverMatch.Groups[1].Value) });
            }

            var databaseMatch = Database.Regex.Match(section);
            if (databaseMatch.Success)
            {
                return (true, nameof(Database), new (string, string)[] { (nameof(PropertyTypes.Database.Name), databaseMatch.Groups[1].Value) });
            }

            var tableMatch = Table.Regex.Match(section);
            if (tableMatch.Success)
            {
                return (true, nameof(Table), new (string, string)[] { 
                    (nameof(PropertyTypes.Table.Name), tableMatch.Groups[1].Value),
                    (nameof(PropertyTypes.Table.Schema), tableMatch.Groups[2].Value)
                });
            }

            var columnMatch = Column.Regex.Match(section);
            if (columnMatch.Success)
            {
                return (true, nameof(Column), new (string, string)[] { (nameof(PropertyTypes.Column.Name), columnMatch.Groups[1].Value) });
            }

            return (false, null, null);
        }
    }

    internal enum ContextLevel : byte
    {
        [Description(Constants.Column_Context)]
        Column = 0,
        [Description(Constants.Table_Context)]
        Table = 1,
        [Description(Constants.Database_Context)]
        Database = 2,
        [Description(Constants.Server_Context)]
        Server = 3
    }
}
