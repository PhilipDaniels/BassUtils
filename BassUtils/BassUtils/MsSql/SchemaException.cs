using System;
using System.Runtime.Serialization;

namespace BassUtils.MsSql
{
    /// <summary>
    /// Indicates an exception with data schema, for example with checking
    /// column names or parsing schema.
    /// </summary>
    [Serializable]
    public class SchemaException : Exception
    {
        /// <summary>
        /// Construct a new exception.
        /// </summary>
        public SchemaException()
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        public SchemaException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// <param name="innerException">Inner exception.</param>
        public SchemaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Construct a new exception using a serialization context.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaing context.</param>
        protected SchemaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
