using System;

namespace BassUtils.NetCore
{
    /// <summary>
    /// Exception thrown for configuration errors.
    /// </summary>
    public class ConfigurationException : Exception
    {
        /// <summary>
        /// Initialize a new instance of the exception.
        /// </summary>
        /// <param name="msg">Exception message.</param>
        public ConfigurationException(string msg) : base(msg)
        {
        }
    }
}
