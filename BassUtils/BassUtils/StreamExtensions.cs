using System.IO;

namespace BassUtils
{
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

            byte[] buffer = new byte[32768];
            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }
    }
}
