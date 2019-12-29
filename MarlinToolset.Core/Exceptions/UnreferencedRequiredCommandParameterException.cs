using MarlinToolset.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarlinToolset.Core.Exceptions
{
    public class UnreferencedRequiredCommandParameterException : MarlinToolsetException
    {
        public IEnumerable<CommandParameter> UnreferencedRequired { get; private set; }

        public UnreferencedRequiredCommandParameterException(
            string message)
            : base(message)
        {
        }

        public UnreferencedRequiredCommandParameterException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        public UnreferencedRequiredCommandParameterException()
            : base($"Command parameter is required.")
        {
        }

        public UnreferencedRequiredCommandParameterException(IEnumerable<CommandParameter> parameters)
            : base($"Command has {parameters.Count()} unreferenced required parameters.")
        {
            UnreferencedRequired = parameters;
        }
    }
}
