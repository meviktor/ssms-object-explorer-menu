using Microsoft.Build.Framework;
using SSMSObjectExplorerMenu.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSObjectExplorerMenu.extendedfiltering
{
    internal class ExtendedFilteringProperties
    {
        internal const byte ContextLevel_Column = 0;
        internal const byte ContextLevel_Table = 1;
        internal const byte ContextLevel_Database = 2;
        internal const byte ContextLevel_Server = 3;

        private Server _server = Server.Any;
        private Database _database = Database.Any;
        private Table _table = Table.Any;
        private Column _column = Column.Any;

        internal Server Server { 
            get { return _server; }
            set {
                if(value is null)
                    throw new ArgumentNullException(nameof(value), $"Parameter '{nameof(value)}' cannot be null.");
                _server = value;
            }
        }

        internal Database Database { 
            get { return _database; }
            set {
                if(value is null)
                    throw new ArgumentNullException(nameof(value), $"Parameter '{nameof(value)}' cannot be null.");
                _database = value;
            }
        }

        internal Table Table { 
            get { return _table; }
            set {
                if(value is null)
                    throw new ArgumentNullException(nameof(value), $"Parameter '{nameof(value)}' cannot be null.");
                _table = value;
            }
        }

        internal Column Column { 
            get { return _column; }
            set {
                if(value is null)
                    throw new ArgumentNullException(nameof(value), $"Parameter '{nameof(value)}' cannot be null.");
                _column = value;
            }
        }

        /// <summary>
        /// Returns the context level the filtering properties are targeting. This means the lowest context level where a condition is defined.
        /// </summary>
        internal byte? TargetContextLevel => 
            Column != Column.Any ? ContextLevel_Column :
            Table != Table.Any ? ContextLevel_Table :
            Database != Database.Any ? ContextLevel_Database :
            Server != Server.Any ? ContextLevel_Server :
            null;

        internal bool IsValid => TargetContextLevel is not null;

        /// <summary>
        /// Checks if the filtering properties of an SSMS Object Explorer node comply to the filtering properties of a <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="subject">The filtering properites built from the navigation context of a node in the SSMS Object Explorer tree.</param>
        /// <param name="expectation">The filtering properties of a <see cref="MenuItem"/> (custom command).</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If source or target is null.</exception>
        internal static bool ComplyTo(this ExtendedFilteringProperties subject, ExtendedFilteringProperties expectation)
        {
            if(subject is null)
                throw new ArgumentNullException(nameof(subject), $"Parameter '{nameof(subject)}' cannot be null.");
            if(expectation is null)
                throw new ArgumentNullException(nameof(target), $"Parameter '{nameof(expectation)}' cannot be null.");
            if (!subject.IsValid)
                throw new ArgumentException(nameof(subject), $"Parameter '{nameof(subject)}' does not filter for anything.");
            if (!expectation.IsValid)
                throw new ArgumentException(nameof(expectation), $"Parameter '{nameof(subject)}' does not filter for anything.");

            if (subject.TargetContextLevel != expectation.TargetContextLevel)
                return false;

            var contextLevel = subject.TargetContextLevel;

            // Column does not match.
            if (contextLevel == ContextLevel_Column && subject.Column.Name != expectation.Column.Name)
                return false;
            // Table does not match. It's only relevant, if the MenuItem's filter:
            // - Targets the table level, or
            // - Targets a column, but also filters for table name/schema
            if ((subject.Table.Name != expectation.Table.Name || subject.Table.Schema != expectation.Table.Schema) &&
               (contextLevel == ContextLevel_Table || (contextLevel < ContextLevel_Table && expectation.Table != Table.Any)))
                return false;
            // Database does not match. It's only relevant, if the MenuItem's filter:
            // - Targets the database level, or
            // - Targets a column or a table, but also filters for database name
            if ((subject.Database.Name != expectation.Database.Name) &&
                (contextLevel == ContextLevel_Database || (contextLevel < ContextLevel_Database && expectation.Database != Database.Any)))
                return false;
            // Server does not match. It's only relevant, if the MenuItem's filter:
            // - Targets the server level, or
            // - Targets a column, a table, or a database, but also filters for server name
            if ((subject.Server.Name != expectation.Server.Name) &&
                (contextLevel == ContextLevel_Server || (contextLevel < ContextLevel_Server && expectation.Server != Server.Any)))
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
            // TODO
            // Split nav context by backslash ('/')
            // If there's any segment not column, table, database, server -> throw ex or return null
            // Segment types cannot be duplicated -> throw ex or return null
            // Visit each segment and gain the nav context component info (name, schema) using regex or some other method
            // Build the ExtendedFilteringProperties instance
            throw new NotImplementedException();
        }
    }

    internal abstract class NameProperty
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
    }

    internal abstract class NameSchemaProperties : NameProperty
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
    }

    internal sealed class Server : NameProperty
    {
        internal static readonly ServerProperties Any = new Server("*");

        internal Server(string name) : base(name) { }

        public override string ToString() => $"Server[@Name='{Name}']";
    }

    internal sealed class Database : NameProperty
    {
        internal static readonly Database Any = new Database("*");

        internal Database(string name) : base(name) { }

        public override string ToString() => $"Database[@Name='{Name}']";
    }

    internal sealed class Table : NameSchemaProperties
    {
        internal static readonly Table Any = new Table("*", "*");

        internal Table(string name, string schema) : base(name, schema) { }

        public override string ToString() => $"Table[@Name='{Name}' and @Schema='{Schema}']";
    }

    internal sealed class Column : NameProperty
    {
        internal static readonly Column Any = new Column("*");

        internal Column(string name) : base(name) { }

        public override string ToString() => $"Column[@Name='{Name}']";
    }
}
