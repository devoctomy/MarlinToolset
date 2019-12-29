using System;
using System.Collections.Generic;
using System.Text;

namespace MarlinToolset.Core.Exceptions
{
    public class InvalidParameterChoiceException : MarlinToolsetException
    {
        public string Token { get; private set; }
        public string Value { get; private set; }
        public IList<string> Choices { get; private set; }

        public InvalidParameterChoiceException(string message)
            : base(message)
        {
        }

        public InvalidParameterChoiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidParameterChoiceException()
            : base($"Command parameter choice is invalid.")
        {
        }

        public InvalidParameterChoiceException(
            string token,
            string value,
            IList<string> choices)
            : base($"Command parameter '{token}' has an invalid value of '{value}'. Must be one of '{string.Join(',', choices)}'.")
        {
            Token = token;
            Value = value;
            Choices = choices;
        }
    }
}
