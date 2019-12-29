using MarlinToolset.Core.Services;
using System;

namespace MarlinToolset.Core.Exceptions
{
    public class UnknownCommandParameterException : MarlinToolsetException
    {
        public string Key { get; private set; }

        public UnknownCommandParameterException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        public UnknownCommandParameterException()
            : base($"Command parameter is unknown.")
        {
        }

        public UnknownCommandParameterException(string key)
            : base($"Command parameter '{key}' is unknown.")
        {
            Key = key;
        }
    }
}
