using System;

namespace Potestas.Exceptions.ProcessorExceptions
{
    public class SaveToSqlProcessorException : Exception
    {
        public SaveToSqlProcessorException() : base() { }

        public SaveToSqlProcessorException(string message) : base(message) { }

        public SaveToSqlProcessorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
