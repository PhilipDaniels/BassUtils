using System.Data;
using Dawn;

namespace ClassLibrary1.Data
{
    /// <summary>
    /// Extensions to the <code>System.Data.DataSet</code> class.
    /// </summary>
    public static class DataSetExtensions
    {
        /// <summary>
        /// Sets all tables in the DataSet to be read only.
        /// </summary>
        /// <param name="dataSet">The DataSet.</param>
        public static void SetReadOnly(this DataSet dataSet)
        {
            dataSet.SetReadOnly(true);
        }

        /// <summary>
        /// Sets the ReadOnly flag on all tables in the DataSet to true or false.
        /// </summary>
        /// <param name="dataSet">The DataSet.</param>
        /// <param name="readOnly">The new value for the read-only flag.</param>
        public static void SetReadOnly(this DataSet dataSet, bool readOnly)
        {
            Guard.Argument(dataSet, nameof(dataSet)).NotNull();

            foreach (DataTable dt in dataSet.Tables)
            {
                dt.SetReadOnly(readOnly);
            }
        }
    }
}
