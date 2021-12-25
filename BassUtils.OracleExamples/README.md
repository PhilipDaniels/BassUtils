# Introduction

This project demonstrates calling Oracle from C# in several ways. In particular, the passing
of complex objects types (UDTs) and collections is demonstrated.

Oracle has a split between "SQL level" and "PL/SQL level" and restrictions in the ODP.Net
driver limit what you can do from C#. The combination of the two is very confusing, and error
messages are often less than helpful. The collection of helper methods in
[BassUtils.Oracle](https://www.nuget.org/packages/BassUtils.Oracle)
together with the code in this project constitute a 'HOW-TO' guide to Oracle data access
from C#.

# Setup

You will need an Oracle instance to play with. The script `CreateObjects.sql` will create
all the Oracle structures necessary. The C# assumes this has been done within a schema
called 'DemoUser' (search and replace if you need to change it).

Probably the easiest way is to install Oracle via Docker - instructions are container
within the SQL file.

The connection string is hardcoded within `Db.cs` and you do not need to install
the Oracle client software (TNS) in order to run the demo.

# Array binding

[Oracle on Array Binding](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/19.3.2/odpnt/featOraCommand.html#GUID-FACB870D-6F8B-46EA-95EA-65C6C6536B9E),

This is the simplest technique. It is handled solely within ODP.Net so you can use it with
existing stored procedures. A SQL statement is constructed with a parameter(s) and sent to
the server. Instead of sending a single value for the parameters an array of values is sent
in one trip. The command is then executed multiple times on the server side.

- **Advantages** It's simple, and effective at reducing the number of network calls
- **Cons** It is still operating row-at-a-time on the server side, and you can only send scalar
  values (numbers, strings etc.)


# PL/SQL Associative Arrays

[Oracle on Associative Arrays](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/19.3.2/odpnt/featOraCommand.html#GUID-05A6D391-E77F-41AF-83A2-FE86A3D98872),

This is the first set-based technique. It allows passing a set of scalars (ints, strings etc.)
between C# and Oracle in one call. It requires a little more setup than array binding, but
offers better server-side performance. It's a good way of passing sets of numbers or strings,
such as might be primary key fields, to Oracle. If more complex data must be passed UDTs are
a better option.

- **Advantages** Easy to use. The types defined are generally applicable and can be reused
  in many procs (since they are just array of integers, strings, dates etc.)
- **Cons** Only works with certain scalar types, tables of which must be declared within a package.
  The arrays are somewhat weird to work with within an Oracle procedure. Confusing error messages
  are produced if you don't get it right.


# Oracle UDTs

[Oracle on UDTs](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/19.3.2/odpnt/featUDTs.html#GUID-7913CDD0-CB22-4257-828F-FBCCA3FE9126)

This is the most powerful of the three techniques presented here. It is the only technique
that allows you to pass large amounts of structured data backwards and forwards between C#
and Oracle.

It utilises the "CREATE TYPE objXYZ AS OBJECT" and "CREATE TYPE tblXYZ AS TABLE OF objXYZ"
syntax in Oracle - unfortunately these must be performed at schema-level, though
procs that use the objects and tables can be defined at schema OR package level.

You may see these referred to as 'nested tables'

An alternative way of creating aggregate types in Oracle, RECORD types, only works inside
packages, and can't be used from C#. So we ignore RECORDs in these examples.

- **Pros** The most powerful technique, allows passing structured data to and from Oracle,
  as a single object, furthermore entire tables of those structures can be passed using
  to parameters. This enables effective set-based IO to and from Oracle, and set-based
  transaction performance on the server.
- **Cons** Requires the creation of a .Net class and specialized binding code. The Oracle types
  must be created at schema level, it won't work if they are defined at package level. However,
  the procs that use those types CAN be defined at package level (thanks, Oracle).


# If Things Aren't Working

If there is a problem in Oracle, use this to find out what's wrong. If the db is small,
just leave out the WHERE clause:

```SQL
SELECT TEXT
FROM DBA_ERRORS
WHERE
    OWNER = 'procedure-owner'
    AND NAME = 'procedure-name'
    AND TYPE = 'PROCEDURE'
ORDER BY LINE
```


# Next Steps

Pull requests demonstrating further techniques are welcome.

I'm particularly considering fleshing out some techniques within PL/SQL itself, such
as how to use pipelined functions.


# Further Info

- [Guide to Oracle SQL](https://docs.oracle.com/en/database/oracle/oracle-database/19/sqlrf/Basic-Elements-of-Oracle-SQL.html)
- [ODP.Net Main Page](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/19.3.2/odpnt/index.html)
- [Data type mapping from C# to Oracle](https://docs.oracle.com/en/database/oracle/oracle-data-access-components/19.3.2/odpnt/featTypes.html#GUID-9DA46E61-DAA9-424A-A35B-10F36E4B6734)

