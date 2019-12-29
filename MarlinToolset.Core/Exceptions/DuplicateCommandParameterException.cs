using MarlinToolset.Core.Services;
using System;

namespace MarlinToolset.Core.Exceptions
{
    public class DuplicateCommandParameterException : MarlinToolsetException
    {
        public CommandParameter Duplicate { get; private set; }

        public DuplicateCommandParameterException(
            string message)
            : base(message)
        {
        }

        public DuplicateCommandParameterException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        public DuplicateCommandParameterException()
            : base($"Command parameter is duplicate.")
        {
        }

        public DuplicateCommandParameterException(CommandParameter parameter)
            : base($"Command parameter '{parameter.Token}' is duplicated.")
        {
            Duplicate = parameter;
        }
    }
}
