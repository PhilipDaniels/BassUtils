using System;

namespace BassUtils.NetCore
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string msg) : base(msg)
        {
        }
    }
}
