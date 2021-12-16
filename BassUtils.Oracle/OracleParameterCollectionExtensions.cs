using System.Data;
using Dawn;
using Oracle.ManagedDataAccess.Client;

namespace BassUtils.Oracle
{
    public static class OracleParameterCollectionExtensions
    {
        public static OracleParameter AddReturnParameter(this OracleParameterCollection parameterCollection, OracleDbType oracleDbType)
        {
            Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();

            var prm = new OracleParameter();
            prm.ParameterName = "RETURN_VALUE";
            prm.Direction = ParameterDirection.ReturnValue;
            prm.OracleDbType = oracleDbType;
            parameterCollection.Add(prm);
            return prm;
        }

        public static OracleParameter AddObjectReturnParameter(this OracleParameterCollection parameterCollection, string udtTypeName)
        {
            Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
            Guard.Argument(udtTypeName, nameof(udtTypeName)).NotNull().NotWhiteSpace();

            var prm = new OracleParameter();
            prm.ParameterName = "RETURN_VALUE";
            prm.Direction = ParameterDirection.ReturnValue;
            prm.OracleDbType = OracleDbType.Object;
            prm.UdtTypeName = udtTypeName;
            parameterCollection.Add(prm);
            return prm;
        }

        public static OracleParameter AddArrayReturnParameter(this OracleParameterCollection parameterCollection, string udtTypeName)
        {
            Guard.Argument(parameterCollection, nameof(parameterCollection)).NotNull();
            Guard.Argument(udtTypeName, nameof(udtTypeName)).NotNull().NotWhiteSpace();

            var prm = new OracleParameter();
            prm.ParameterName = "RETURN_VALUE";
            prm.Direction = ParameterDirection.ReturnValue;
            prm.OracleDbType = OracleDbType.Array;
            prm.UdtTypeName = udtTypeName;
            parameterCollection.Add(prm);
            return prm;
        }
    }
}