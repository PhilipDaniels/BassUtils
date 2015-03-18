namespace BassUtils
{
    /// <summary>
    /// Specifies how to treat System.Nullable types when returning them from the ObjectDataReader.
    /// </summary>
    public enum NullConversion
    {
        /// <summary>
        /// Return System.Nullable of T as System.Nullable of T.
        /// Null values are returned as "null".
        /// n.b. Such data readers cannot be loaded into DataTable objects.
        /// </summary>
        None,

        /// <summary>
        /// Return System.Nullable of T as T. (e.g. int? => int).
        /// Null values are returned as DBNull.Value.
        /// This makes the reader safe for loading into a DataTable object.
        /// </summary>
        ToDBNull
    }
}
