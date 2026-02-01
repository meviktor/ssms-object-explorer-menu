using SSMSObjectExplorerMenu.objects;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

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
            if(server is null)
                throw new ArgumentNullException(nameof(server), $"Parameter '{nameof(server)}' cannot be null.");

            Server = server;
            Database = database;
            Table = table;
            Column = column;
        }

        /// <summary>
        /// Returns the context level the filtering properties are targeting. This means the lowest context level where a condition is defined.
        /// </summary>
        internal ExtendedFilteringContextLevel? ContextLevel
        {
            get
            {
                if (Column != Column.Any) return ExtendedFilteringContextLevel.Column;
                if (Table != Table.Any) return ExtendedFilteringContextLevel.Table;
                if (Database != Database.Any) return ExtendedFilteringContextLevel.Database;
                if (Server != Server.Any) return ExtendedFilteringContextLevel.Server;

                return null;
            }
        }

        internal bool IsValid => ContextLevel != null;

        /// <summary>
        /// Returns the filter in string representation, resembling the navigation context format.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Server}{(Database != null ? $"/{Database}" : null)}{(Table != null ? $"/{Table}" : null)}{(Column != null ? $"/{Column}" : null)}";

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

            if (subject.ContextLevel != expectation.ContextLevel)
                return false;

            var contextLevel = subject.ContextLevel;

            // Column does not match while targeting column level.
            if (contextLevel == ExtendedFilteringContextLevel.Column && subject.Column.Name != expectation.Column.Name)
                return false;
            // Table does not match. It's only relevant, if the MenuItem's filter:
            // - Targets the table level, or
            // - Targets a column, but also filters for table name/schema
            if ((subject.Table?.Name != expectation.Table?.Name || subject.Table?.Schema != expectation.Table?.Schema) &&
               (contextLevel == ExtendedFilteringContextLevel.Table || (contextLevel < ExtendedFilteringContextLevel.Table && expectation.Table != Table.Any)))
                return false;
            // Database does not match. It's only relevant, if the MenuItem's filter:
            // - Targets the database level, or
            // - Targets a column or a table, but also filters for database name
            if ((subject.Database?.Name != expectation.Database?.Name) &&
                (contextLevel == ExtendedFilteringContextLevel.Database || (contextLevel < ExtendedFilteringContextLevel.Database && expectation.Database != Database.Any)))
                return false;
            // Server does not match. It's only relevant, if the MenuItem's filter:
            // - Targets the server level, or
            // - Targets a column, a table, or a database, but also filters for server name
            if ((subject.Server?.Name != expectation.Server?.Name) &&
                (contextLevel == ExtendedFilteringContextLevel.Server || (contextLevel < ExtendedFilteringContextLevel.Server && expectation.Server != Server.Any)))
                return false;

            // All checks passed. Object Explorer node complies to MenuItem filtering properties.
            return true;
        }

        /// <summary>
        /// Builds an <see cref="ExtendedFilteringProperties"/> instance from a navigation context string.
        /// </summary>
        /// <param name="navigationContext">The navigaton context belongs to the SSMS Object Explorer tree node.</param>
        /// <returns></returns>
        internal static ExtendedFilteringProperties BuildFomNavigationContext(string navigationContext)
        {
            var navContextSections = navigationContext.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var parsedSections = navContextSections.Select(s => IdentifySection(s)).ToArray();
            if (parsedSections.Any(s => !s.IsIdentified))
            {
                throw new ArgumentException($"The navigation context '{navigationContext}' contains unrecognized sections. Accepted section types: {nameof(Server)}, {nameof(Database)}, {nameof(Table)}, {nameof(Column)}.");
            }

            var duplicatedSectionKinds = parsedSections.GroupBy(s => s.Kind).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
            if(duplicatedSectionKinds.Any())
            {
                throw new ArgumentException($"The navigation context '{navigationContext}' contains duplicated sections: {string.Join(", ", duplicatedSectionKinds)}.");
            }

            var filter = new ExtendedFilteringProperties();

            var columnSection = parsedSections.SingleOrDefault(s => s.Kind == nameof(Column));
            var tableSection = parsedSections.SingleOrDefault(s => s.Kind == nameof(Table));
            var databaseSection = parsedSections.SingleOrDefault(s => s.Kind == nameof(Database));
            var serverSection = parsedSections.SingleOrDefault(s => s.Kind == nameof(Server));

            filter.Column = columnSection != default ? new Column(columnSection.Properties.Single().Value) : null;
            filter.Table = tableSection != default ? new Table(
                tableSection.Properties.Single(p => p.Name == nameof(extendedfiltering.Table.Name)).Value,
                tableSection.Properties.Single(p => p.Name == nameof(extendedfiltering.Table.Schema)).Value)
                    : (filter.Column != null ? Table.Any : null);
            filter.Database = databaseSection != default ? new Database(databaseSection.Properties.Single().Value) 
                    : (filter.Table != null ? Database.Any : null);
            filter.Server = serverSection != default ? new Server(serverSection.Properties.Single().Value)
                    : (filter.Database != null ? Server.Any : null);

            return filter.IsValid ? filter : null;
        }

        private static (bool IsIdentified, string Kind, (string Name, string Value)[] Properties) IdentifySection(string section)
        {
            var serverMatch = Server.Regex.Match(section);
            if (serverMatch.Success)
            {
                return (true, nameof(Server), new (string, string)[] { (nameof(extendedfiltering.Server.Name), serverMatch.Groups[1].Value) });
            }

            var databaseMatch = Database.Regex.Match(section);
            if (databaseMatch.Success)
            {
                return (true, nameof(Database), new (string, string)[] { (nameof(extendedfiltering.Database.Name), databaseMatch.Groups[1].Value) });
            }

            var tableMatch = Table.Regex.Match(section);
            if (tableMatch.Success)
            {
                return (true, nameof(Table), new (string, string)[] { 
                    (nameof(extendedfiltering.Table.Name), tableMatch.Groups[1].Value),
                    (nameof(extendedfiltering.Table.Schema), tableMatch.Groups[2].Value)
                });
            }

            var columnMatch = Column.Regex.Match(section);
            if (columnMatch.Success)
            {
                return (true, nameof(Column), new (string, string)[] { (nameof(extendedfiltering.Column.Name), columnMatch.Groups[1].Value) });
            }

            return (false, null, null);
        }
    }

    internal enum ExtendedFilteringContextLevel : byte
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

    internal abstract class NameProperty : IEquatable<NameProperty>
    {
        internal string Name { get; private set; }

        internal NameProperty(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), $"Parameter '{nameof(name)}' cannot be null or whitespace.");
            }
            Name = name;
        }

        public bool Equals(NameProperty other)
        {
            if(ReferenceEquals(this, other))
                return true;
            if (other is null)
                return false;
            if (other.GetType() != this.GetType())
                return false;
            return string.Equals(this.Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj) => Equals(obj as NameProperty);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Name);

        public static bool operator ==(NameProperty left, NameProperty right)
            => ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

        public static bool operator !=(NameProperty left, NameProperty right)
            => !(left == right);
    }

    internal abstract class NameSchemaProperties : NameProperty, IEquatable<NameSchemaProperties>
    {
        internal string Schema { get; private set; }

        internal NameSchemaProperties(string name, string schema) : base(name)
        {
            if (string.IsNullOrWhiteSpace(schema))
            {
                throw new ArgumentNullException(nameof(schema), $"Parameter '{nameof(schema)}' cannot be null.");
            }
            Schema = schema;
        }

        public bool Equals(NameSchemaProperties other) => base.Equals(other)
            && string.Equals(this.Schema, other.Schema, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object other) => Equals(other as NameSchemaProperties);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + base.GetHashCode();
                hash = hash * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(Schema);
                return hash;
            }
        }
    }

    internal sealed class Server : NameProperty
    {
        internal static readonly Server Any = new Server("*");
        internal static readonly Regex @Regex = new Regex(@"Server\[\@Name\='([a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)'\]");

        internal Server(string name) : base(name) { }

        public override string ToString() => $"Server[@Name='{Name}']";
    }

    internal sealed class Database : NameProperty
    {
        internal static readonly Database Any = new Database("*");
        internal static readonly Regex @Regex = new Regex(@"Database\[\@Name\='([a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)'\]");

        internal Database(string name) : base(name) { }

        public override string ToString() => $"Database[@Name='{Name}']";
    }

    internal sealed class Table : NameSchemaProperties
    {
        internal static readonly Table Any = new Table("*", "*");
        internal static readonly Regex @Regex = new Regex(@"Table\[\@Name\='([a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)' and \@Schema\='([a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)'\]");

        internal Table(string name, string schema) : base(name, schema) { }

        public override string ToString() => $"Table[@Name='{Name}' and @Schema='{Schema}']";
    }

    internal sealed class Column : NameProperty
    {
        internal static readonly Column Any = new Column("*");
        internal static readonly Regex @Regex = new Regex(@"Column\[\@Name\='([a-zA-Z_\@#][a-zA-Z0-9_\@#\$]*)'\]");

        internal Column(string name) : base(name) { }

        public override string ToString() => $"Column[@Name='{Name}']";
    }
}
