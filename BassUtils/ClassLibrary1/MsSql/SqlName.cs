﻿using System;
using System.Diagnostics;
using Dawn;

namespace ClassLibrary1.MsSql
{
    /// <summary>
    /// Represents a quoted name of a SQL object.
    /// Pass in a .-separated string, e.g. "Database.Schema.TableName".
    /// The name can have between 1 and 4 parts, the latter meaning a
    /// linked-server object.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public class SqlName
    {
        /// <summary>
        /// The left-hand character used by MS SQL to quote identifiers.
        /// </summary>
        public const string MSSqlLeftQuote = "[";

        /// <summary>
        /// The right-hand character used by MS SQL to quote identifiers.
        /// </summary>
        public const string MSSqlRightQuote = "]";

        /// <summary>
        /// The left hand side quote that the name was created with.
        /// </summary>
        public string LeftQuote { get; private set; }

        /// <summary>
        /// The right hand side quote that the name was created with.
        /// </summary>
        public string RightQuote { get; private set; }

        /// <summary>
        /// The unquoted name of the linked server, or null.
        /// </summary>
        public string LinkedName { get; private set; }

        /// <summary>
        /// The unquoted name of the database, or null.
        /// </summary>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// The unquoted name of the schema, or null.
        /// </summary>
        public string SchemaName { get; private set; }

        /// <summary>
        /// The unquoted name of the object, or null.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Constructs a new SqlName using the MS SQL quote characters.
        /// </summary>
        /// <param name="name">Name.</param>
        public SqlName(string name)
            : this(name, MSSqlLeftQuote, MSSqlRightQuote)
        {
        }

        /// <summary>
        /// Constructs a new <c>SqlName</c> using the specified quote characters.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="leftQuote">Left hand side quote.</param>
        /// <param name="rightQuote">Right hand side quote.</param>
        public SqlName(string name, string leftQuote, string rightQuote)
        {
            Guard.Argument(name, nameof(name)).NotNull();
            Guard.Argument(leftQuote, nameof(leftQuote)).NotNull().NotWhiteSpace();
            Guard.Argument(rightQuote, nameof(rightQuote)).NotNull().NotWhiteSpace();

            LeftQuote = leftQuote;
            RightQuote = rightQuote;

            if (name.Length == 0)
            {
                LinkedName = DatabaseName = SchemaName = Name = null;
                return;
            }

            string[] parts = name.Split(new char[] { '.' });

            switch (parts.Length)
            {
                case 1:
                    LinkedName = null;
                    DatabaseName = null;
                    SchemaName = "dbo";
                    Name = TrimSqlQuotes(parts[0], LeftQuote, RightQuote);
                    break;
                case 2:
                    LinkedName = null;
                    DatabaseName = null;
                    SchemaName = TrimSqlQuotes(parts[0], LeftQuote, RightQuote);
                    Name = TrimSqlQuotes(parts[1], LeftQuote, RightQuote);
                    break;
                case 3:
                    LinkedName = null;
                    DatabaseName = TrimSqlQuotes(parts[0], LeftQuote, RightQuote);
                    SchemaName = TrimSqlQuotes(parts[1], LeftQuote, RightQuote);
                    Name = TrimSqlQuotes(parts[2], LeftQuote, RightQuote);
                    break;
                case 4:
                    LinkedName = TrimSqlQuotes(parts[0], LeftQuote, RightQuote);
                    DatabaseName = TrimSqlQuotes(parts[1], LeftQuote, RightQuote);
                    SchemaName = TrimSqlQuotes(parts[2], LeftQuote, RightQuote);
                    Name = TrimSqlQuotes(parts[3], LeftQuote, RightQuote);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Invalid name: " + name);
            }
        }

        /// <summary>
        /// Returns the quoted one-part-name, consisting of just the <code>Name</code>.
        /// </summary>
        public string OnePartName
        {
            get
            {
                return Quote(Name);
            }
        }

        /// <summary>
        /// Returns the quoted two-part-name, consisting of just the <code>SchemaName</code>
        /// and the <code>Name</code>.
        /// </summary>
        public string TwoPartName
        {
            get
            {
                return Quote(SchemaName) + "." + Quote(Name);
            }
        }

        /// <summary>
        /// Returns the quoted three-part-name, consisting of the <code>DatabaseName</code>, <code>SchemaName</code>
        /// and the <code>Name</code>.
        /// </summary>
        public string ThreePartName
        {
            get
            {
                return Quote(DatabaseName) + "." + Quote(SchemaName) + "." + Quote(Name);
            }
        }

        /// <summary>
        /// Returns the quoted full four-part name of the object.
        /// </summary>
        public string FourPartName
        {
            get
            {
                return Quote(LinkedName) + "." + Quote(DatabaseName) + "." + Quote(SchemaName) + "." + Quote(Name);
            }
        }

        /// <summary>
        /// Returns the smallest possible quoted name (i.e. omitting blank parts)
        /// of the object.
        /// </summary>
        /// <returns>Name.</returns>
        public override string ToString()
        {
            if (SchemaName == null)
                return Name;
            else if (DatabaseName == null)
                return TwoPartName;
            else if (LinkedName == null)
                return ThreePartName;
            else
                return FourPartName;
        }

        /// <summary>
        /// Removes the "[" and "]" SQL quote characters if they exist.
        /// </summary>
        /// <param name="name">SQL name. Can be 1, 2 or N part.</param>
        /// <returns>Name with the "[" and "]" removed.</returns>
        public static string TrimSqlQuotes(string name)
        {
            return TrimSqlQuotes(name, MSSqlLeftQuote, MSSqlRightQuote);
        }

        /// <summary>
        /// Removes the left and right SQL quote characters if they exist.
        /// </summary>
        /// <param name="name">SQL name. Can be 1, 2 or N part.</param>
        /// <param name="leftQuote">Left quote character.</param>
        /// <param name="rightQuote">Right quote character.</param>
        /// <returns>Name with the quotes removed.</returns>
        public static string TrimSqlQuotes(string name, string leftQuote, string rightQuote)
        {
            Guard.Argument(name, nameof(name)).NotNull();
            Guard.Argument(leftQuote, nameof(leftQuote)).NotNull().NotWhiteSpace();
            Guard.Argument(rightQuote, nameof(rightQuote)).NotNull().NotWhiteSpace();

            return name.Replace(leftQuote, "").Replace(rightQuote, "");
        }

        string Quote(string name)
        {
            Guard.Argument(name, nameof(name)).NotNull();

            return LeftQuote + name + RightQuote;
        }
    }
}
