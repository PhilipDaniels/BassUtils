namespace BassUtils
{
    /// <summary>
    /// Format things in human-readable form.
    /// </summary>
    public class Humanizer
    {
        /// <summary>
        /// Formats a number of bytes as a human-readable string.
        /// Copy of https://www.somacon.com/p576.php
        /// which is in the public domain.
        /// The default format is "0.### XB", e.g. "4.2 KB" or "1.434 GB"
        /// </summary>
        /// <param name="numBytes">The number of bytes.</param>
        /// <param name="numDecimalPlaces">The number of decimal places.</param>
        /// <returns>Human-readable string representation of the number of bytes.</returns>
        public static string FormatBytes(long numBytes, byte numDecimalPlaces = 1)
        {
            long i = numBytes;

            // Get absolute value
            long absolute_i = i < 0 ? -i : i;

            // Determine the suffix and readable value
            string suffix;
            double readable;

            if (absolute_i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = i >> 50;
            }
            else if (absolute_i >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = i >> 40;
            }
            else if (absolute_i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = i >> 30;
            }
            else if (absolute_i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = i >> 20;
            }
            else if (absolute_i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = i >> 10;
            }
            else if (absolute_i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = i;
            }
            else
            {
                return i.ToString("0 B"); // Byte
            }

            // Divide by 1024 to get fractional value
            readable /= 1024;

            // Return formatted number with suffix
            var formatString = "0." + "#".Repeat(numDecimalPlaces) + " ";
            return readable.ToString(formatString) + suffix;
        }
    }
}
