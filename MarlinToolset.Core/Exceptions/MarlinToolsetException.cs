using System;

namespace MarlinToolset.Core.Exceptions
{
    public class MarlinToolsetException : Exception
    {
        public MarlinToolsetException()
        {
        }

        public MarlinToolsetException(string message)
            : base(message)
        {
        }

        public MarlinToolsetException(
            string message,
            Exception exception)
            : base(message, exception)
        {
        }
    }
}
