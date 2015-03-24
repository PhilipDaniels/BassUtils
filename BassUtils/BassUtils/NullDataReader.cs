using System;
using System.Data;

namespace BassUtils
{
    /// <summary>
    /// The NullDataReader implements the IDataReader interface, but will never return any rows:
    /// The Read() method will always return false. IsClosed always returns True. Many other methods
    /// will throw exceptions if you call them.
    /// </summary>
    public sealed class NullDataReader : IDataReader
    {
        /// <summary>
        /// Closes the data reader (a no-op).
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// Always returns 0.
        /// </summary>
        public int Depth
        {
            get { return 0; }
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <returns>Never, always throws.</returns>
        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Always returns true.
        /// </summary>
        public bool IsClosed
        {
            get { return true; }
        }

        /// <summary>
        /// Always returns false.
        /// </summary>
        public bool NextResult()
        {
            return false;
        }

        /// <summary>
        /// Always returns false.
        /// </summary>
        public bool Read()
        {
            return false;
        }

        /// <summary>
        /// Always returns 0.
        /// </summary>
        public int RecordsAffected
        {
            get { return 0; }
        }

        /// <summary>
        /// Disposes the data reader.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Always returns 0.
        /// </summary>
        public int FieldCount
        {
            get { return 0; }
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The index within the row from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>Never, always throws.</returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldoffset">The index within the row from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>Never, always throws.</returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>Never, always throws.</returns>
        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public object GetValue(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="values">An array of System.Object to copy the attribute fields into.</param>
        /// <returns>Never, always throws.</returns>
        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>Never, always throws.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "Should never be called, so raising exception is the proper thing to do.")]
        public object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Throws NotImplementedException.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>Never, always throws.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "Should never be called, so raising exception is the proper thing to do.")]
        public object this[int i]
        {
            get { throw new NotImplementedException(); }
        }
    }

}
