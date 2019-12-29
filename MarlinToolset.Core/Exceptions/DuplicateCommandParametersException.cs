using MarlinToolset.Core.Services;
using System;

namespace MarlinToolset.Core.Exceptions
{
    public class DuplicateCommandParametersException : MarlinToolsetException
    {
        public CommandParameter Duplicate { get; private set; }

        public DuplicateCommandParametersException(
            string message)
            : base(message)
        {
        }

        public DuplicateCommandParametersException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        public DuplicateCommandParametersException()
            : base($"Command has duplicate parameters.")
        {
        }

        public DuplicateCommandParametersException(CommandParameter parameter)
            : base($"Command parameter '{parameter.Token}' is duplicated.")
        {
            Duplicate = parameter;
        }
    }
}
