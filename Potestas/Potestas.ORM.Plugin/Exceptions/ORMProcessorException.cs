using System;

namespace Potestas.ORM.Plugin.Exceptions
{
    public class ORMProcessorException : Exception
    {
        public ORMProcessorException() : base() { }

        public ORMProcessorException(string message) : base(message) { }

        public ORMProcessorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
