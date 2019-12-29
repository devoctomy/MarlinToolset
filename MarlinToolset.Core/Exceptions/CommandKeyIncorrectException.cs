using System;
using System.Collections.Generic;
using System.Text;

namespace MarlinToolset.Core.Exceptions
{
    public class CommandKeyIncorrectException : MarlinToolsetException
    {
        public string Actual { get; private set; }
        public string Expected { get; private set; }

        public CommandKeyIncorrectException(
            string message)
            : base(message)
        {
        }

        public CommandKeyIncorrectException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        public CommandKeyIncorrectException()
            : base($"Command Key incorrect.")
        {
        }

        public CommandKeyIncorrectException(
            string actual,
            string expected)
            : base($"Command Key incorrect, expected '{expected}' but got '{actual}'.")
        {
            Actual = actual;
            Expected = expected;
        }
    }
}
