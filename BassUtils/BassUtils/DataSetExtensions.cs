using System.Data;

namespace BassUtils
{
    public static class DataSetExtensions
    {
        public static void SetReadOnly(this DataSet dataSet)
        {
            dataSet.SetReadOnly(true);
        }

        public static void SetReadOnly(this DataSet dataSet, bool readOnly)
        {
            dataSet.ThrowIfNull("dataSet");

            foreach (DataTable dt in dataSet.Tables)
            {
                dt.SetReadOnly(readOnly);
            }
        }
    }
}
