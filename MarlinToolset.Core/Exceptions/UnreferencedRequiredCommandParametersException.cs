﻿using MarlinToolset.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarlinToolset.Core.Exceptions
{
    public class UnreferencedRequiredCommandParametersException : MarlinToolsetException
    {
        public IEnumerable<CommandParameter> UnreferencedRequired { get; private set; }

        public UnreferencedRequiredCommandParametersException(
            string message)
            : base(message)
        {
        }

        public UnreferencedRequiredCommandParametersException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        public UnreferencedRequiredCommandParametersException()
            : base($"Command has unreferenced required parameters.")
        {
        }

        public UnreferencedRequiredCommandParametersException(IEnumerable<CommandParameter> parameters)
            : base($"Command has {parameters.Count()} unreferenced required parameters.")
        {
            UnreferencedRequired = parameters;
        }
    }
}
