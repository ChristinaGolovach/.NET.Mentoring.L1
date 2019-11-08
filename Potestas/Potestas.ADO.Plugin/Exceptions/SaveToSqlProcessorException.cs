using System;

namespace Potestas.ADO.Plugin.Exceptions
{
    public class SaveToSqlProcessorException : Exception
    {
        public SaveToSqlProcessorException() : base() { }

        public SaveToSqlProcessorException(string message) : base(message) { }

        public SaveToSqlProcessorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
