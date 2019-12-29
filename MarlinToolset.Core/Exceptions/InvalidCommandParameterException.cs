using System;

namespace MarlinToolset.Core.Exceptions
{
    public class InvalidCommandParameterException : MarlinToolsetException
    {
        public string Key { get; private set; }

        public InvalidCommandParameterException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidCommandParameterException()
            : base($"Command parameter is invalid.")
        {
        }

        public InvalidCommandParameterException(string key)
            : base($"Command parameter '{key}' is invalid.")
        {
            Key = key;
        }
    }
}
