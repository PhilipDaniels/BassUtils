using BassUtils.Oracle;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace BassUtils.OracleExamples.OracleUDTs;

/// <summary>
/// A type that maps to the schema-level objPerson.
/// </summary>
public class PersonRecord : IOracleCustomType, INullable
{
    public override string ToString()
    {
        return $"{FirstName} {LastName}, Age {Age}, Last Updated on {UpdatedDate}";
    }

    [OracleObjectMapping("AGE")]
    public int? Age { get; set; }

    [OracleObjectMapping("FIRSTNAME")]
    public string FirstName { get; set; }

    [OracleObjectMapping("LASTNAME")]
    public string LastName { get; set; }

    [OracleObjectMapping("NOTE")]
    public string Note { get; set; }

    [OracleObjectMapping("UPDATEDDATE")]
    public DateTime? UpdatedDate { get; set; }

    private bool objectIsNull;
    public bool IsNull => objectIsNull;
    public static PersonRecord Null => new() { objectIsNull = true };

    public void FromCustomObject(OracleConnection con, object udt)
    {
        con.SetUdtValue(udt, "AGE", Age);
        con.SetUdtValue(udt, "FIRSTNAME", FirstName);
        con.SetUdtValue(udt, "LASTNAME", LastName);
        con.SetUdtValue(udt, "NOTE", Note);
        con.SetUdtValue(udt, "UPDATEDDATE", UpdatedDate);
    }

    public void ToCustomObject(OracleConnection con, object udt)
    {
        Age = con.GetUdtNullableInt32(udt, "AGE");
        FirstName = con.GetUdtNullableString(udt, "FIRSTNAME");
        LastName = con.GetUdtNullableString(udt, "LASTNAME");
        Note = con.GetUdtNullableString(udt, "NOTE");
        UpdatedDate = con.GetUdtNullableDateTime(udt, "UPDATEDDATE");
    }
}


/// <summary>
/// An Oracle factory for the PersonRecord type.
/// This allows us to bind/create individual objects.
/// Note the attribute. This is how we link our type for Oracle ODP.Net.
/// </summary>
[OracleCustomTypeMapping("DemoUser.objPerson")]
public class PersonRecordFactory : IOracleCustomTypeFactory
{
    public IOracleCustomType CreateObject() => new PersonRecord();
}


/// <summary>
/// An aggregate (Oracle always calls this 'Array') irrespective of
/// the fact that we are dealing with a nested table type in the Oracle Db.
/// </summary>
public class PersonRecordArray : IOracleCustomType, INullable
{
    [OracleArrayMapping]
    public PersonRecord[] Rows;

    private bool objectIsNull;
    public bool IsNull => objectIsNull;
    public static PersonRecordArray Null => new() { objectIsNull = true };

    public void FromCustomObject(OracleConnection con, object udt)
    {
        OracleUdt.SetValue(con, udt, 0, Rows);
    }

    public void ToCustomObject(OracleConnection con, object udt)
    {
        Rows = (PersonRecord[])OracleUdt.GetValue(con, udt, 0);
    }
}

/// <summary>
/// An Oracle factory for the PersonRecordArray type.
/// This allows us to bind/create arrays of PersonRecords.
/// </summary>
[OracleCustomTypeMapping("DemoUser.tblPerson")]
public class PersonRecordArrayFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
{
    public IOracleCustomType CreateObject()
    {
        // Here we return the aggregate type.
        return new PersonRecordArray();
    }

    public Array CreateArray(int numElems)
    {
        // Here we return a .Net Array type of the record type.
        return new PersonRecord[numElems];
    }

    public Array CreateStatusArray(int numElems)
    {
        return null;
    }
}
