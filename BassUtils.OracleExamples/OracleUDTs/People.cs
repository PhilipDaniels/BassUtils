namespace BassUtils.OracleExamples.OracleUDTs;

public class People
{
    public static PersonRecord MakeLeela()
    {
        return new PersonRecord() { Age = 25, FirstName = "Turanga", LastName = "Leela" };
    }

    public static PersonRecord[] MakeFuturama()
    {
        return new PersonRecord[]
        {
            new PersonRecord() { Age = 22, FirstName = "Philip", LastName = "Fry" },
            new PersonRecord() { Age = 123, FirstName = "Hubert", LastName = "Farnsworth" },
            new PersonRecord() { Age = 35, FirstName = "Zap", LastName = "Brannigan" }
        };
    }
}
