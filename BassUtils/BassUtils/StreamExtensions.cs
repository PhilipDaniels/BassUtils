using System.IO;

namespace BassUtils
{
    /// <summary>
    /// Extensions to the <code>System.IO.Stream</code> class.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Completely reads a stream from its current position and returns the data as an array of bytes.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns>Array of bytes from the stream.</returns>
        public static byte[] ReadFully(this Stream stream)
        {
            stream.ThrowIfNull("stream");

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
