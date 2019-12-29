using MarlinToolset.Core.Services;
using System;

namespace MarlinToolset.Core.Exceptions
{
    public class UnknownCommandParametersException : MarlinToolsetException
    {
        public string Key { get; private set; }

        public UnknownCommandParametersException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        public UnknownCommandParametersException()
            : base($"Command has one or more unknown parameters.")
        {
        }

        public UnknownCommandParametersException(string key)
            : base($"Command parameter '{key}' is unknown.")
        {
            Key = key;
        }
    }
}
