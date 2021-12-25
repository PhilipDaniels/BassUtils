/*
 * Run this script on your Oracle database to create the objects
 * needed by this program.
 * 
 * If you want to run this on a play system you can install Oracle 12c
 * using Docker.
 * 
 * Get Oracle running in the container.
 *   docker run -d -it --name oracle -p 1521:1521 store/oracle/database-enterprise:12.0.0.1
 *   docker ps
 *   docker exec -it oracle bash -c "source /home/oracle/.bashrc; sqlplus /nolog"
 * 
 * The above runs without volumes, so data is not persisted after restarts of the
 * docker container. To use volumes do one of these - the first mounts the OracleDBData
 * volume in the standard docker volume directory, the second uses an explicit volume
 * somewhere in the host file system:
 *   docker run -d -it --name oracle -p 1521:1521 -v OracleDBData:/ORCL store/oracle/database-enterprise:12.0.0.1
 *   docker run -d -it --name oracle -p 1521:1521 -v /home/me/somevolume:/ORCL store/oracle/database-enterprise:12.0.0.1
 * The former will be somewhere like /var/lib/docker/volumes... - it can be found with
 *   docker volume inspect OracleDBData
 * The second form is therefore preferred.
 * 
 * Then in SQLPlus
 *   connect sys as sysdba;    						// default pwd is Oradoc_db1
 *   alter session set "_ORACLE_SCRIPT"=true;
 *   create user demouser identified by demouser;  	// the second demouser is the pwd
 *   grant connect, resource, dba to demouser;		// make them a dba
 *   grant all privileges to demouser;              // alternative!
 *   select name from v$database;          			// shows ORCLCDB, note the 2nd C.
 * 
 * I was running Oracle on a Linux host and connecting to it from inside
 * a Virtual Box VM running Windows.
 *
 * To connect from inside Virtual Box in DBeaver you need to first setup
 * port forwarding for the port 1521 (the default Oracle port).
 * Then create a new DB connection in DBeaver, with the host set to the
 * default gateway (type ipconfig in a command window), database set to
 * ORCLCDB, and choose SID rather than Service Name.
 */


CREATE OR REPLACE PROCEDURE DropIfExistsInner(pObjectType VARCHAR2, pObjectName VARCHAR2, pErrorCode PLS_INTEGER)
AS
BEGIN
    BEGIN
        EXECUTE IMMEDIATE 'DROP ' || pObjectType || ' ' || pObjectName;
    EXCEPTION
        WHEN OTHERS THEN
            IF SQLCODE != pErrorCode THEN
                RAISE;
            END IF;
    END;
END DropIfExistsInner;

  
CREATE OR REPLACE PROCEDURE DropIfExists(pObjectType VARCHAR2, pObjectName VARCHAR2) AS
BEGIN
    -- See https://stackoverflow.com/questions/1799128/oracle-if-table-exists
    CASE pObjectType
    WHEN 'TABLE' THEN
        DropIfExistsInner(pObjectType, pObjectName, -942);
    WHEN 'SEQUENCE' THEN
        DropIfExistsInner(pObjectType, pObjectName, -2289);
    WHEN 'VIEW' THEN
        DropIfExistsInner(pObjectType, pObjectName, -942);
    WHEN 'TRIGGER' THEN
        DropIfExistsInner(pObjectType, pObjectName, -4080);
    WHEN 'INDEX' THEN
        DropIfExistsInner(pObjectType, pObjectName, -1418);
    WHEN 'DATABASE LINK' THEN
        DropIfExistsInner(pObjectType, pObjectName, -2024);
    WHEN 'MATERIALIZED VIEW' THEN
        DropIfExistsInner(pObjectType, pObjectName, -12003);
    WHEN 'TYPE' THEN
        DropIfExistsInner(pObjectType, pObjectName, -4043);
    WHEN 'USER' THEN
        DropIfExistsInner(pObjectType, pObjectName, -1918);
    WHEN 'PACKAGE' THEN
        DropIfExistsInner(pObjectType, pObjectName, -4043);
    WHEN 'PROCEDURE' THEN
        DropIfExistsInner(pObjectType, pObjectName, -4043);
    WHEN 'FUNCTION' THEN
        DropIfExistsInner(pObjectType, pObjectName, -4043);
    WHEN 'TABLESPACE' THEN
        DropIfExistsInner(pObjectType, pObjectName, -959);
    WHEN 'SYNONYM' THEN
        DropIfExistsInner(pObjectType, pObjectName, -1434);
    ELSE
        RAISE CASE_NOT_FOUND;
    END CASE;
END DropIfExists;


BEGIN
    -- Safely drop everything first to avoid problems with dependency cycles.
    -- The aim is to make the script idempotent.
    -- Drop everything in the reverse order that this script creates them in.
	DropIfExists('PACKAGE', 'MyPackage');
	DropIfExists('FUNCTION', 'FunctionWithScalarParameters');
	DropIfExists('PACKAGE', 'COLLECTION_TYPES');
  	DropIfExists('TYPE', 'objManager');
  	DropIfExists('TYPE', 'tblPerson');
  	DropIfExists('TYPE', 'objPerson');
  	DropIfExists('TYPE', 'tblInteger');
  	DropIfExists('TABLE', 'Person');
END;


-- ================================================================================
CREATE TABLE Person
  (
  Age NUMBER,
  FirstName VARCHAR2(255),
  LastName VARCHAR2(255),
  Note VARCHAR2(255),
  UpdatedDate DATE DEFAULT SYSDATE
  );
 

INSERT INTO Person(Age, FirstName, LastName, Note) VALUES (60, 'George', 'Clooney', NULL);
INSERT INTO Person(Age, FirstName, LastName, Note) VALUES (55, 'Salma', 'Hayek', NULL);
INSERT INTO Person(Age, FirstName, LastName, Note) VALUES (64, 'Pete', 'Postlethwaite', 'Dead');
INSERT INTO Person(Age, FirstName, LastName, Note) VALUES (62, 'Sean', 'Bean', 'Alive');


-- ================================================================================
-- Create schema-level nested table of simple built-in scalar types.
-- This is only for information, it is not used within these examples.
--
-- These types can be used for columns within other tables, objects, variables, parameters etc.
-- HOWEVER, there is no easy way of passing them from .Net and associative arrays
-- (TABLE ... INDEX BY ...) CANNOT BE CREATED at schema-level.
--
-- Conclusion: If you want to pass a collection of a simple scalar type you have two options.
-- The first is to define an associative array type inside a package and use that, the
-- second is to write a wrapper UDT (CREATE TYPE x AS OBJECT) that has only a single
-- field (say an int or a varchar2). The second technique is very long-winded and is not
-- explored in these examples.
CREATE TYPE tblInteger IS TABLE OF INTEGER;


-- ================================================================================
-- Create a schema-level UDT object type and corresponding table of that type.
-- These types can be used for columns within other tables, variables, parameters etc
-- and they can be referenced within packages.
--
-- There is another aggregate type called a record (TYPE x IS RECORD ...) which
-- CANNOT BE CREATED AT SCHEMA LEVEL, they must be created inside a package HOWEVER
-- associative arrays of records cannot be used from ODP.Net.
-- See https://docs.oracle.com/en/database/oracle/oracle-database/18/odpnt/featOraCommand.html#GUID-05A6D391-E77F-41AF-83A2-FE86A3D98872
--
-- Therefore if we want to pass tables of aggregate types from C# to Oracle we must
-- create OBJECTs like this, and define in the C# a corresponding class and annotate it
-- with special Oracle attributes so that ODP.Net knows how to serialize them.
CREATE TYPE objPerson AS OBJECT
  (
  Age INTEGER,
  FirstName VARCHAR2(255),
  LastName VARCHAR2(255),
  Note VARCHAR2(255),
  UpdatedDate DATE
  );

-- Create a schema-level collection type (nested table type) of another schema-level type.
CREATE TYPE tblPerson AS TABLE OF objPerson;

-- Shows how types can be nested in other types. Not used in these examples.
CREATE TYPE objManager AS OBJECT
  (
  Person objPerson,
  DirectReports tblPerson
  );
  
 
-- ================================================================================
-- Generic associative array types for use in any proc.
-- These can be referenced in schema-level procs assuming the package exists first.
--
-- Note: From ODP.NET you can only use AAs of scalar types, not records! See
--   https://docs.oracle.com/en/database/oracle/oracle-database/18/odpnt/featOraCommand.html#GUID-05A6D391-E77F-41AF-83A2-FE86A3D98872
--
-- This package declares arrays of all the types you can use from ODP.Net.
CREATE PACKAGE COLLECTION_TYPES IS
	TYPE arrBinaryFloat IS TABLE OF BINARY_FLOAT INDEX BY BINARY_INTEGER;
	TYPE arrChar IS TABLE OF CHAR INDEX BY BINARY_INTEGER;
	TYPE arrDate IS TABLE OF DATE INDEX BY BINARY_INTEGER;
	TYPE arrNChar IS TABLE OF NCHAR INDEX BY BINARY_INTEGER;
	TYPE arrNumber IS TABLE OF NUMBER INDEX BY BINARY_INTEGER;
	TYPE arrNIdentifier IS TABLE OF NVARCHAR2(30) INDEX BY BINARY_INTEGER;
	TYPE arrNVarcharMax IS TABLE OF NVARCHAR2(32767) INDEX BY BINARY_INTEGER;
	TYPE arrRaw IS TABLE OF RAW(50) INDEX BY BINARY_INTEGER;
	TYPE arrRowId IS TABLE OF ROWID INDEX BY BINARY_INTEGER;
	TYPE arrURowId IS TABLE OF UROWID INDEX BY BINARY_INTEGER;
	TYPE arrIdentifier IS TABLE OF VARCHAR2(30) INDEX BY BINARY_INTEGER;
	TYPE arrVarcharMax IS TABLE OF VARCHAR2(32767) INDEX BY BINARY_INTEGER;

	-- It is also possible to declare tables of types that you CAN'T use from ODP.Net,
	-- i.e. PL/SQL types such as BOOLEAN. These can only be used in PL/SQL.
	-- See the above link for what is actually usable. These don't work from ODP.Net,
	-- for example.
	-- TYPE arrBoolean IS TABLE OF BOOLEAN INDEX BY BINARY_INTEGER;
	-- TYPE arrBinaryDouble IS TABLE OF BINARY_DOUBLE INDEX BY BINARY_INTEGER;
END COLLECTION_TYPES;


-- ================================================================================
-- Starting simple, passing scalar values. This demonstates data-type mapping
-- from C# to Oracle using type inference. See
-- 	 https://docs.oracle.com/en/database/oracle/oracle-database/19/sqlrf/Data-Types.html#GUID-7B72E154-677A-4342-A1EA-C74C1EA928E6
-- for a description of Oracle SQL types.
-- This is a schema-level function, but the same code will work for a package-level
-- function too.
CREATE OR REPLACE FUNCTION FunctionWithScalarParameters
	(
	pInteger IN INTEGER,
	pNumber IN NUMBER,
	pDate IN OUT DATE,					-- To demonstrate precision
	pTimestamp IN OUT TIMESTAMP,		-- To demonstrate precision
	pChar IN CHAR,
	pNChar IN NCHAR,
	pAsciiString IN VARCHAR2,
	pUnicodeString IN NVARCHAR2,
	pFloat IN BINARY_FLOAT,
	pDouble IN BINARY_DOUBLE,
	pOutputString OUT VARCHAR2
	) RETURN NUMBER
AS
BEGIN
	pOutputString := pChar || ',' || pNChar || ',' || pAsciiString || ',' || pUnicodeSTring || ',' || pFloat || ',' || pDouble;
	RETURN 42;
END FunctionWithScalarParameters;


-- ================================================================================
CREATE PACKAGE MyPackage AS
	-- This demonstrates passing arrays (they are associative arrays) of scalars
	-- to a package procedure and getting them back.
	FUNCTION FunctionTakingArraysOfScalars
    	(
    	pFloats COLLECTION_TYPES.arrBinaryFloat,
    	pDates COLLECTION_TYPES.arrDate,
	    pNumbers COLLECTION_TYPES.arrNumber,
	    pStrings COLLECTION_TYPES.arrIdentifier,
	    pNumbersOut OUT COLLECTION_TYPES.arrNumber,
	    pStringsOut OUT COLLECTION_TYPES.arrIdentifier
    	) RETURN COLLECTION_TYPES.arrIdentifier;
    
    -- Demonstrate client-side array binding. This is a technique to insert multiple
	-- data items while making fewer network calls.
    PROCEDURE InsertPerson(pAge INTEGER, pFirstName VARCHAR2, pLastName VARCHAR2);
   
   	-- Retrieving people using SYS_REFCURSOR.
   	FUNCTION GetPeople RETURN SYS_REFCURSOR;
	PROCEDURE GetPeopleNames(pFirstNames OUT SYS_REFCURSOR, pLastNames OUT SYS_REFCURSOR);

	-- Passing and returning UDTs.
	FUNCTION FunctionTakingObjects(pPerson objPerson) RETURN objPerson;

	-- Passing and returning tables of UDTs.
	FUNCTION FunctionTakingTablesOfObjects
		(
		pPeople tblPerson,
		pAgeMultipliers COLLECTION_TYPES.arrNumber
		) RETURN tblPerson;
	
	-- Passing and retrieving null UDTs (including in tables).
	PROCEDURE ProcWithNullableObjects
		(
		pPerson1 IN OUT objPerson,
  		pPerson2 IN OUT objPerson,
  		pPeople IN OUT tblPerson
		);	
END MyPackage;

CREATE PACKAGE BODY MyPackage AS
	-- This demonstrates passing arrays (they are associative arrays) of scalars
	-- to a package procedure and getting them back.
	-- Useful for passing primary key values, for example, to operate on a set
	-- of rows rather than one row at a time.
	--
	-- Note that according to the Oracle documentation at
	-- https://docs.oracle.com/en/database/oracle/oracle-database/18/odpnt/featOraCommand.html#GUID-05A6D391-E77F-41AF-83A2-FE86A3D98872
	-- not all of the Oracle scalar types can be used in associative arrays
	-- from ODP.Net.
	-- 
	-- This proc is in a package, but it could just as easily be defined at
	-- schema-level.
	FUNCTION FunctionTakingArraysOfScalars
    	(
    	pFloats COLLECTION_TYPES.arrBinaryFloat,
	    pDates COLLECTION_TYPES.arrDate,
    	pNumbers COLLECTION_TYPES.arrNumber,
	    pStrings COLLECTION_TYPES.arrIdentifier,
	    pNumbersOut OUT COLLECTION_TYPES.arrNumber,
	    pStringsOut OUT COLLECTION_TYPES.arrIdentifier
	    ) RETURN COLLECTION_TYPES.arrIdentifier
	AS
		vNumericResults COLLECTION_TYPES.arrNumber;
		vStringResults COLLECTION_TYPES.arrIdentifier;
  	BEGIN
	  	-- Watch out! Associative arrays that you want to use from C# must
	  	-- be indexed starting at 1.
	  	pNumbersOut(1) := 20;
	  	pNumbersOut(2) := 33;
	  	pNumbersOut(3) := 44;

	  	pStringsOut(1) := 'Returned';
	  	pStringsOut(2) := 'From Oracle';
	  
	  	vNumericResults(1) := pDates.COUNT;
	  	vNumericResults(2) := pNumbers.COUNT;
	 	vNumericResults(3) := pStrings.COUNT;
	  	
	 	vStringResults(1) := 'A';
	 	vStringResults(2) := 'string';
	 	vStringResults(3) := 'array';
	 	vStringResults(4) := NULL;
	 	vStringResults(5) := 'from';
	 	vStringResults(6) := 'oracle';
	 
    	RETURN vStringResults;
  	END FunctionTakingArraysOfScalars;
  
  	-- Demonstrate client-side array binding. This is a technique to insert multiple
	-- data items while making fewer network calls.
  	PROCEDURE InsertPerson(pAge INTEGER, pFirstName VARCHAR2, pLastName VARCHAR2)
	AS
	BEGIN
  		INSERT INTO Person(Age, FirstName, LastName)
  		VALUES (pAge, pFirstName, pLastName);
	END InsertPerson;

	-- Retrieving people using SYS_REFCURSOR.
	FUNCTION GetPeople RETURN SYS_REFCURSOR
	AS
		curReturn SYS_REFCURSOR;
	BEGIN
		OPEN curReturn FOR
		SELECT *
		FROM DemoUser.Person
		ORDER BY FirstName, LastName;
		
		RETURN curReturn;
	END GetPeople;

	-- Retrieving people using SYS_REFCURSOR.
	PROCEDURE GetPeopleNames(pFirstNames OUT SYS_REFCURSOR, pLastNames OUT SYS_REFCURSOR)
	AS
	BEGIN
		OPEN pFirstNames FOR
		SELECT FirstName
		FROM Person
		ORDER BY FirstName, LastName;
	
		OPEN pLastNames FOR
		SELECT LastName
		FROM Person
		ORDER BY FirstName, LastName;
	END GetPeopleNames;

	-- Passing and returning UDTs.
	-- This is still a scalar function, we are dealing with single objects,
	-- not tables. Note that pPerson is an IN parameter, which means it is
	-- readonly, so we must make a temporary variable in order to change it.
	FUNCTION FunctionTakingObjects(pPerson objPerson) RETURN objPerson
	AS
		vPerson objPerson := pPerson;
	BEGIN
	  	vPerson.Age := pPerson.Age * 2;
	  	vPerson.LastName := UPPER(pPerson.LastName);
	  	RETURN vPerson;
	END;

	-- Passing and returning tables of UDTs.
	FUNCTION FunctionTakingTablesOfObjects
		(
		pPeople tblPerson,
		pAgeMultipliers COLLECTION_TYPES.arrNumber
		) RETURN tblPerson
	AS
		vPeople tblPerson := pPeople;
	BEGIN
	  	-- Extend the table by 1 element so that we can add the extra person. 
  		vPeople.EXTEND();
  		vPeople(vPeople.LAST) := NEW objPerson
			(
			Age => 19,
			FirstName => 'Zaphod',
			LastName => 'Beeblebrox',
			Note => 'Galactic President',
			UpdatedDate => SYSDATE
			);
		
		FOR idx IN vPeople.FIRST..vPeople.LAST LOOP
    		vPeople(idx).Age := vPeople(idx).Age * pAgeMultipliers(Idx);
    		vPeople(idx).LastName := UPPER(vPeople(idx).LastName);
  		END LOOP;
		
	  	RETURN vPeople;
	END;

	-- Passing and retrieving null UDTs (including in tables).
	PROCEDURE ProcWithNullableObjects
		(
		pPerson1 IN OUT objPerson,
  		pPerson2 IN OUT objPerson,
  		pPeople IN OUT tblPerson
		)
	AS
	BEGIN
		-- Create pPerson1 but leave pPerson2 NULL for the caller to deal with.
		IF pPerson1 IS NULL THEN
    		pPerson1 := NEW objPerson
      			(
      			Age => 19,
      			FirstName => 'Zaphod',
      			LastName => 'Beeblebrox',
      			Note => 'Galactic President',
      			UpdatedDate => SYSDATE
      			);
  		END IF;
  
  		-- Fill in the last entry. There should still be
  		-- a null element just before this one for the caller
  		-- to deal with.
  		pPeople(pPeople.LAST) := NEW objPerson
      		(
      		Age => 37,
      		FirstName => 'Marvin',
      		LastName => 'The Paranoid Android',
      		Note => 'Parking Attendant',
      		UpdatedDate => SYSDATE
      		);
	END ProcWithNullableObjects;
END MyPackage;

