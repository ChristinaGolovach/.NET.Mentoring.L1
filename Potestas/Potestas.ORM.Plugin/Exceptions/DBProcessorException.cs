using System;

namespace Potestas.ORM.Plugin.Exceptions
{
    public class DBProcessorException : Exception
    {
        public DBProcessorException() : base() { }

        public DBProcessorException(string message) : base(message) { }

        public DBProcessorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
